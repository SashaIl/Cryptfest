using API.Data.Entities.Wallet;
using API.Data.Entities.WalletEntities;
using AutoMapper;
using Cryptfest.Data.Entities.ClientRequest;
using Cryptfest.Enums;
using Cryptfest.Interfaces.Repositories;
using Cryptfest.Interfaces.Services;
using Cryptfest.Model.Dtos;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Net.Http;
using System.Text.Json;

namespace Cryptfest.ServiceImpementation;

public class ApiService : IApiService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly ICryptoAssetRepository _cryptoAssetRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _conf;

    public ApiService(IHttpClientFactory httpClient, ICryptoAssetRepository cryptoAssetRepository, IMapper mapper, IConfiguration conf)
    {
        _httpClient = httpClient;
        _cryptoAssetRepository = cryptoAssetRepository;
        _mapper = mapper;
        _conf = conf;
    }

    public ApiAccessDto GetApiKeyToken()
    {
        try
        {
            ApiAccessDto output = new()
            {
                Key = _conf["ApiTokens:Crypto:Key"]!,
                Token = _conf["ApiTokens:Crypto:Token"]!
            };
            return output;
        }
        catch { throw; }
    }
    public string GetTop30AssetUrl()
    {
        try
        {
            string output = _conf["ApiLinks:Top30Assets"]!;
            return output;
        }
        catch { throw; }
    }

    public string GetLatestDataUrl()
    {
        try
        {
            string output = _conf["ApiLinks:Latest"]!;
            return output;
        }
        catch { throw; }
    }


    public string GetSpecifiedAsset()
    {
        try
        {
            string output = _conf["ApiLinks:SpecifiedAsset"]!;
            return output;
        }
        catch { throw; }
    }


    public async Task<ToClientDto> UpdateMarketDataInDbAsync(Guid walletId)
    {
        List<CryptoAsset> cryptoAssets = await _cryptoAssetRepository.GetCryptoAssetsAsync();

        ClientRequest? request = (await _cryptoAssetRepository.GetWalletByIdAsync(walletId))?.User.ClientRequest;
        if (request == null)
        {
            return new()
            {
                Message = "wallet id does not exist",
                Status = ResponseStatus.Fail,
            };
        }

        if ((DateTime.UtcNow - request.DateTime).TotalSeconds < 60)
        {
            List<CryptoAssetDto> assetsDto = _mapper.Map<List<CryptoAssetDto>>(cryptoAssets);

            return new()
            {
                Status = ResponseStatus.Success,
                Data = assetsDto
            };
        }


        HttpClient client = _httpClient.CreateClient();

        var keyAndToken = GetApiKeyToken();
        client.DefaultRequestHeaders.Add($"{keyAndToken.Key}", $"{keyAndToken.Token}");
        string latestDataUrl = GetLatestDataUrl();

        try
        {
            HttpResponseMessage response = await client.GetAsync(latestDataUrl);
            response.EnsureSuccessStatusCode();

            var receivedJson = await response.Content.ReadAsStringAsync();

            JsonElement doc = JsonDocument.Parse(receivedJson).RootElement;

            var jsonData = doc.GetProperty("data");

            // variables for loop 
            CryptoAsset? asset = null;
            decimal price;
            decimal PercentChange1h, PercentChange24h, PercentChange7d, PercentChange30d, PercentChange60d;
            JsonElement forPrice;


            foreach (var item in jsonData.EnumerateArray())
            {
                string symbol = item.GetProperty("symbol").GetString()!;
                asset = cryptoAssets.FirstOrDefault(x => x.Symbol == symbol);

                if (asset is not null)
                {
                    forPrice = item.GetProperty("quote").GetProperty("USD");
                    forPrice.GetProperty("price").TryGetDecimal(out price);
                    forPrice.GetProperty("percent_change_1h").TryGetDecimal(out PercentChange1h);
                    forPrice.GetProperty("percent_change_24h").TryGetDecimal(out PercentChange24h);
                    forPrice.GetProperty("percent_change_7d").TryGetDecimal(out PercentChange7d);
                    forPrice.GetProperty("percent_change_30d").TryGetDecimal(out PercentChange30d);
                    forPrice.GetProperty("percent_change_60d").TryGetDecimal(out PercentChange60d);


                    // update assets info, if there are not asset, then create new instance
                    if (asset.MarketData != null)
                    {
                        asset.MarketData.CurrPrice = price;
                        asset.MarketData.PercentChange1h = PercentChange1h;
                        asset.MarketData.PercentChange24h = PercentChange24h;
                        asset.MarketData.PercentChange7d = PercentChange7d;
                        asset.MarketData.PercentChange30d = PercentChange30d;
                        asset.MarketData.PercentChange60d = PercentChange60d;
                    }
                    else
                    {
                        asset.MarketData = new CryptoAssetMarketData()
                        {
                            CurrPrice = price,
                            PercentChange1h = PercentChange1h,
                            PercentChange24h = PercentChange24h,
                            PercentChange7d = PercentChange7d,
                            PercentChange30d = PercentChange30d,
                            PercentChange60d = PercentChange60d,
                        };
                    }

                }
            }

            List<CryptoAssetDto> cryptoAssetsResult = _mapper.Map<List<CryptoAssetDto>>(cryptoAssets);

            request.DateTime = DateTime.UtcNow; 
            bool saveResult = await _cryptoAssetRepository.SaveChangesAsync();


            if (saveResult == true)
            {
                return new()
                {
                    Status = ResponseStatus.Success,
                    Data = cryptoAssetsResult
                };
            }
            else
            {
                return new()
                {
                    Status = ResponseStatus.Fail,
                    Message = "Failed to save to database"
                };
            }
        }
        catch (HttpRequestException ex)
        {
            return new()
            {
                Message = "Failed to get data from external API",
                Status = ResponseStatus.Fail,
            };
        }

        catch (Exception ex)
        {
            return new()
            {
                Message = $"Unexpected error: {ex.Message}",
                Status = ResponseStatus.Fail,
            };
        }
    }
}
