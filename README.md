# ASP .NET 7.0 Authentification 🔒

<strong>This repo contains a simple authentification microservice addable to your project containing a simple login & registration system with other cool & secured tool for more efficienty.</strong>

## Microservices

- AuthService : Managing authentification with tokens and secure cookies </br>
- OtherService : Containing User model (use this service according to your needs) it contains only one model used to manage user informations and show you the purpose of micro-services and how to use it</br>
- ApiGateway : Acts as a single point of entry for customers who wish to access different services of your application </br>

## Cool Features 🧙‍♂️

- JWT authorization and using Claims for API calls
- Session creation and keeping sessionId in secured and signed cookie
- JWT stored in secured and signed cookie
- Middlewares verifying JWT & Cookies integrity
- using cryptography methods

<strong>Cookies are signed with secret keys in `appsettings.json`. When passing by Cookie middleware, it generate hash based on cookie value with secret key. hash freshly created is compared with cookie hash and return boolean value if cookie was changed or not.</strong>

<strong>SessionId is store in secured cookie with same mecanism as above. SessionId is encrypt with AES keys and can be used in API calls just as in `isConnected()` method able to return if user is connected.</strong>

---

## Tech's I used

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white) ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white) ![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)


## Before launching project 🚨

- <strong>Make sure all .NET tools to use ASP.NET core environnement are installed on your machine <br>

- Make sure that SQL server and SQL server management studio are installed on your machine <br> 
GET THEM : <a href="https://www.microsoft.com/en-us/sql-server/sql-server-downloads">SQL Express</a> & <a href="https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16">SSMS</a>

- Install Entity Framework on your machine</strong>

---

## Launch project :

- clone project on your laptop with `git clone https://github.com/Yekuuun/Asp-Net-Authentification.git`

1. <strong>Api Gateway</strong>

- go to ApiGateway folder with `cd ApiGateway`
- restore dependencies with `dotnet restore`
- `dotnet watch run` to run the project

<strong>I decided to use port 5000 for api gateway but you can change it in `Properties` folder
  
---
  
2. <strong> Auth Service</strong>

- go to AuthService with `cd MicroServices/AuthService`
- restore dependencies with dotnet restore
- create `Migrations` folder to contain your migrations
- create first migration with `dotnet ef migrations add "MigrationName" `
- use `dotnet ef database update` to apply changements to your local DB
- `dotnet watch run` to run the project
  
<strong>You can change ports by going into `Properties` file</strong>
</br>

---

3. <strong> Other Service</strong>

- go to AuthService with `cd MicroServices/OtherService`
- restore dependencies with dotnet restore
- create `Migrations` folder to contain your migrations
- create first migration with `dotnet ef migrations add "MigrationName" `
- use `dotnet ef database update` to apply changements to your local DB
- `dotnet watch run` to run the project
  
<strong>You can change ports by going into `Properties` file</strong>
</br>
<strong>This folder contain code related to other service than AuthService and was implemented to give an example of how to communicate with AuthService</strong>


---

## Dependencies :

- AutoMapper
- EntityFrameworkCore
- EntityFrameworkCore.Design
- EntityFrameworkCore.SqlServer
- JWT
- Cookies
- JSON deserializer
- Swashbukle.Filter

-> see it : <a href="https://github.com/Yekuuun/Asp-Net-Authentification/blob/main/MicroServices/AuthService/AuthService.csproj">DEPENDENCIES</a>

</br>

## Learn about microservices with .NET ?

-> see it : <a href="https://www.youtube.com/watch?v=DgVjEo3OGBI&t=3135s">Youtube video</a>

