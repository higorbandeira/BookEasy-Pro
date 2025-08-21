FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["backend/API.BookEasy.Pro.csproj", "backend/"]
RUN dotnet restore "backend/API.BookEasy.Pro.csproj"
COPY . .
RUN dotnet publish "backend/API.BookEasy.Pro.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "API.BookEasy.Pro.dll"]