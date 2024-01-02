#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Payslip.API/Payslip.API.csproj", "Payslip.API/"]
COPY ["Payslip.Application/Payslip.Application.csproj", "Payslip.Application/"]
COPY ["Payslip.Core/Payslip.Core.csproj", "Payslip.Core/"]
COPY ["Payslip.Infrastructure/Payslip.Infrastructure.csproj", "Payslip.Infrastructure/"]
RUN dotnet restore "Payslip.API/Payslip.API.csproj"
COPY . .
WORKDIR "/src/Payslip.API"
RUN dotnet build "Payslip.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payslip.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payslip.API.dll"]