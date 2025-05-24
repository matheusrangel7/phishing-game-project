# Usa imagem base do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos e restaura as dependências
COPY . ./
RUN dotnet restore

# Compila a aplicação
RUN dotnet publish -c Release -o out

# Usa imagem de runtime mais leve
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expõe a porta padrão
EXPOSE 80

# Comando de inicialização
ENTRYPOINT ["dotnet", "PhishingGameWebTest.dll"]
