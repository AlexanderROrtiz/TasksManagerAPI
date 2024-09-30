FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Copia solo los archivos .csproj primero para aprovechar la cach� de Docker
COPY TaskManagement.Api/TaskManagement.Api.csproj TaskManagement.Api/
COPY TaskManagement.Application/TaskManagement.Application.csproj TaskManagement.Application/
COPY TaskManagement.Domain/TaskManagement.Domain.csproj TaskManagement.Domain/
COPY TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj TaskManagement.Infrastructure/

# Copia el resto de los archivos del proyecto
COPY . .

# Restaura las dependencias
RUN dotnet restore TaskManagement.Api/TaskManagement.Api.csproj

# Publica la aplicaci�n
RUN dotnet publish TaskManagement.Api/TaskManagement.Api.csproj -c Release -o out --self-contained true --runtime linux-x64 --framework net8.0 /p:RuntimeFrameworkVersion=8.0.0

# Fase final: imagen de ejecuci�n
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Establece el directorio de trabajo en la imagen final
WORKDIR /app

# Copia los archivos publicados desde la fase de construcci�n
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "TaskManagement.Api.dll"]
