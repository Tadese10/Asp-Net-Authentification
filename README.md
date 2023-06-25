# ASP .NET 7.0 Authentification 🔒

<strong>Complete and simple authentification implementation in a microservice based infrastructure addable to your project. Containing a simple login & registration system with cryptographic methods and tools for more security. Adding Ocelot as Api gateway and synchronous method for communication between user service and authentification service. Adding all this solution in a docker environnement./strong>

## Microservices

- AuthService : Managing authentification with tokens and secure cookies + session handler </br>
- OtherService : Containing User model (use this service according to your needs) it contains only one model used to manage user informations and show you the purpose of micro-services and how to use it</br>
- ApiGateway : Acts as a single point of entry for customers who wish to access different services of your application </br>

## Cool Features 🧙‍♂️

- JWT authorization and restrictions
- Using claims in requests
- Session creation and keeping sessionId in secured and signed cookie
- JWT stored in secured and signed cookie
- Middlewares verifying JWT & Cookies integrity
- Using cryptography methods to hash cookie values

<strong>Cookies are signed with secret keys in `appsettings.json`. When passing by Cookie middleware, it generate hash based on cookie value with secret key. hash freshly created is compared with cookie hash and return boolean value if cookie was changed or not.</strong>

<strong>SessionId is store in secured cookie with same mecanism as above. SessionId is encrypt with AES keys and can be used in API calls just as in `isConnected()` method able to return if user is connected.</strong>

---

## Tech's I used

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white) ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white) ![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)


## Before launching project 🚨

- <strong>Make sure all .NET tools to use ASP.NET core environnement are installed on your machine <br>

- Make sure that SQL server is installed on your machine and you have a tool to see & manage DB data (SSMS, DbBeaver etc.)

- Install globally Entity Framework on your machine with `dotnet tool install --global dotnet-ef` and use `dotnet ef --version`</strong>

- <strong>Make sure that you have docker installed and running on your machine !</strong>

---

## Launch project :

- Clone project on your laptop with `git clone https://github.com/Yekuuun/Asp-Net-Authentification.git`

- Open a new terminal and run ´docker-compose -f docker-compose.develop.yml up´ to install all dependencies and launch docker solution
(if you have problems with docker you can remove all your old images and containers with `docker system prune -a` & `docker volume prune` )

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

