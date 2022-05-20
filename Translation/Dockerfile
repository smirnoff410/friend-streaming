FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /app
EXPOSE 80

COPY "Translation/Translation.csproj" "Translation/Translation.csproj"
COPY "Translation/appsettings.json" "Translation/appsettings.json"

RUN dotnet restore Translation/Translation.csproj

COPY ./Translation ./Translation
COPY ./protos ./protos

RUN dotnet publish Translation/Translation.csproj --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "Translation.dll"]