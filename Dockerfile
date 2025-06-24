FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY The-Scouts.sln .
COPY The-Scouts/The-Scouts.csproj ./The-Scouts/

RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/The-Scouts
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "The-Scouts.dll"]
