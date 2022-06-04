FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ../../src/. ./src/
RUN dotnet restore

# copy everything else and build app
WORKDIR /app/src/Dhgms.AspNetCoreContrib.Example.WebSite
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/Dhgms.AspNetCoreContrib.Example.WebSite/out ./
ENTRYPOINT ["dotnet", "dhgms.aspnetcorecontrib.example.website.dll"]