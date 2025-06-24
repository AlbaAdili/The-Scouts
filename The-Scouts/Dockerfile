# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY The-Scouts.sln ./
COPY The-Scouts/The-Scouts.csproj ./The-Scouts/

# Restore dependencies
RUN dotnet restore The-Scouts.sln

# Copy the rest of the project files
COPY . .

# Publish the application
WORKDIR /src/The-Scouts
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "The-Scouts.dll"]
