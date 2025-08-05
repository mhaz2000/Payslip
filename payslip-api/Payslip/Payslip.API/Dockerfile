# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Payslip.API/Payslip.API.csproj", "Payslip.API/"]
COPY ["Payslip.Application/Payslip.Application.csproj", "Payslip.Application/"]
COPY ["Payslip.Core/Payslip.Core.csproj", "Payslip.Core/"]
COPY ["Payslip.Infrastructure/Payslip.Infrastructure.csproj", "Payslip.Infrastructure/"]
RUN dotnet restore "Payslip.API/Payslip.API.csproj"

# Copy everything else including ClientSide folder
COPY . .

WORKDIR "/src/Payslip.API"
RUN dotnet build "Payslip.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Payslip.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy published app files
COPY --from=publish /app/publish .

# Copy ClientSide folder with built frontend files into the image (inside /app/ClientSide)
COPY Payslip.API/ClientSide ./ClientSide

ENTRYPOINT ["dotnet", "Payslip.API.dll"]
