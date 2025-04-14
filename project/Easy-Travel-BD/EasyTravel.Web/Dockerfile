FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
RUN apt update && apt install -y nodejs
COPY ["EasyTravel.Web/EasyTravel.Web.csproj", "EasyTravel.Web/"]
COPY ["EasyTravel.Domain/EasyTravel.Domain.csproj", "EasyTravel.Domain/"]
COPY ["EasyTravel.Application/EasyTravel.Application.csproj", "EasyTravel.Application/"]
COPY ["EasyTravel.Infrastructure/EasyTravel.Infrastructure.csproj", "EasyTravel.Infrastructure/"]
RUN dotnet restore "EasyTravel.Web/EasyTravel.Web.csproj"

COPY . .
WORKDIR "/src/EasyTravel.Web"
RUN dotnet build "EasyTravel.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EasyTravel.Web.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "EasyTravel.Web.dll"]
