# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["PizzaDelivery.API.csproj", "./"]

# Restore dependencies
RUN dotnet restore "PizzaDelivery.API.csproj"

# Copy source code
COPY . .

# Build application
RUN dotnet build "PizzaDelivery.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PizzaDelivery.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Set environment
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

# Start application
ENTRYPOINT ["dotnet", "PizzaDelivery.API.dll"]
