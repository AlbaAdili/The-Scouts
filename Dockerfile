FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the .csproj and restore
COPY The-Scouts/The-Scouts.csproj ./The-Scouts/
RUN dotnet restore ./The-Scouts/The-Scouts.csproj

# Copy everything else and publish
COPY . .
WORKDIR /app/The-Scouts
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "The-Scouts.dll"]
