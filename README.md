Description
-
This backend was built to handle cryptocurrency features on the client side. It's written in ASP.NET, and the project is organized into clear folders so everything is easy to find.
DTOs are separated, and the project also includes JWT authentication and SMTP email sending. The backend integrates with an external API to handle certain operations.

---------
Project Structure:
-
/AutoMapperProfiles        -> AutoMapper profiles 

/Controllers               -> Controllers

/Data                      -> Database entities and context

/Enums                     -> Enums

/Interfaces                -> Service and repository interfaces

/Migrations                -> Ef core migrations

/Model                     -> DTOs

/Repositories              -> Database operations

/ServiceImplementation     -> Service implementations

/Validation                -> Custom validation

appsettings*.json          -> Configuration files

Program.cs                 -> Entry point

---------

How to start the project:
-
1) Clone the repository
2) Create these three files inside the project:
   - appsettings.json
   - appsettings.apitokens.json
   - appsettings.apilinks.json
Add your data following the appsettings.example*.json files.

3) Add your database connection string in appsettings.json (you can check the example in appsettings.example.json)
4) dotnet run
