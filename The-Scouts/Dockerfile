# Use the official .NET 8 SDK image to build and publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy .csproj and restore as distinct layers
COPY *.sln .
COPY The-Scouts/*.csproj ./The-Scouts/
RUN dotnet restore

# Copy the entire project and build
COPY . .
WORKDIR /app/The-Scouts
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variable to run the app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "The-Scouts.dll"]
