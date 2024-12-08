# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY . ./

# Restore dependencies
RUN dotnet restore library-management.csproj

# Build the app
RUN dotnet publish library-management.csproj -c Release -o out

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the port your app uses
EXPOSE 5000

# Start the application
ENTRYPOINT ["dotnet", "library-management.dll"]
