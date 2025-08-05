# Use ASP.NET Core 8.0 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["Payslip.API/Payslip.API.csproj", "Payslip.API/"]
COPY ["Payslip.Application/Payslip.Application.csproj", "Payslip.Application/"]
COPY ["Payslip.Core/Payslip.Core.csproj", "Payslip.Core/"]
COPY ["Payslip.Infrastructure/Payslip.Infrastructure.csproj", "Payslip.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Payslip.API/Payslip.API.csproj"

# Copy all source including ClientSide folder
COPY . .

# Build the project
WORKDIR "/src/Payslip.API"
RUN dotnet build "Payslip.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Payslip.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Copy the ClientSide folder to the image (optional: only if needed at runtime)
COPY Payslip.API/ClientSide ./ClientSide

# Set environment variable for ASP.NET Core URLs
ENV ASPNETCORE_URLS=http://+:80

# Run the application
ENTRYPOINT ["dotnet", "Payslip.API.dll"]
