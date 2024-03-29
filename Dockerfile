# Etap 1: Budowa aplikacji
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Kopiowanie kodu źródłowego do kontenera
COPY . /source

# Ustawienie katalogu roboczego
WORKDIR /source/TagList

# Wykonanie publish
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

# Etap 2: Finalny obraz
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Ustawienie katalogu roboczego
WORKDIR /app

# Instalacja icu-libs i icu-data-full
RUN apk add --no-cache icu-libs && \
    apk add --no-cache icu-data-full

# Kopiowanie skompilowanej aplikacji z poprzedniego etapu
COPY --from=build /app .

# Ustawienie punktu wejścia
ENTRYPOINT ["dotnet", "TagList.dll"]
