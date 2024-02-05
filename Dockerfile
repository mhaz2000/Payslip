# Stage 1: Build Node.js app
FROM node:20 AS payslip_client_build

WORKDIR /app/payslip_client

COPY payslip/package*.json ./
RUN npm install

COPY payslip .
RUN npm run build

# Stage 2: Build .NET Core app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS payslip_api_build

WORKDIR /app/payslip_api

COPY payslip-api/Payslip/Payslip.API/Payslip.API.csproj Payslip.API/
COPY payslip-api/Payslip/Payslip.Application/Payslip.Application.csproj Payslip.Application/
COPY payslip-api/Payslip/Payslip.Core/Payslip.Core.csproj Payslip.Core/
COPY payslip-api/Payslip/Payslip.Infrastructure/Payslip.Infrastructure.csproj Payslip.Infrastructure/

RUN dotnet restore Payslip.API/Payslip.API.csproj

COPY payslip-api/Payslip .
RUN dotnet build Payslip.API/Payslip.API.csproj -c Release -o /app/payslip_api/build

# Stage 3: Combine Node.js and .NET Core apps
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS final

WORKDIR /app

# Copy built payslip_client files
COPY --from=payslip_client_build /app/payslip_client ./payslip_client

# Copy built payslip_api files
COPY --from=payslip_api_build /app/payslip_api/build ./payslip_api

# Expose ports
EXPOSE 3000
EXPOSE 80

# Install node runtime
RUN apt-get update && apt-get install -y nodejs && apt-get install -y npm

# Start both Node.js and .NET Core apps
CMD cd /app/payslip_client && npm start & cd /app/payslip_api && dotnet Payslip.API.dll
