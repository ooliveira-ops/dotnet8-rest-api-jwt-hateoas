FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY RestWithASPNETUdemy/RestWithASPNETUdemy/*.csproj ./RestWithASPNETUdemy/RestWithASPNETUdemy/
RUN dotnet restore ./RestWithASPNETUdemy/RestWithASPNETUdemy/RestWithASPNETUdemy.csproj

COPY . .
RUN dotnet publish RestWithASPNETUdemy/RestWithASPNETUdemy/RestWithASPNETUdemy.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RestWithASPNETUdemy.dll"]