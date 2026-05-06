FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY FitLife.Classes.slnx .
COPY FitLife.Classes.Api/FitLife.Classes.Api.csproj FitLife.Classes.Api/
COPY FitLife.Classes.Tests/FitLife.Classes.Tests.csproj FitLife.Classes.Tests/
RUN dotnet restore FitLife.Classes.slnx

COPY . .
RUN dotnet test FitLife.Classes.Tests/FitLife.Classes.Tests.csproj --no-restore
RUN dotnet publish FitLife.Classes.Api/FitLife.Classes.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FitLife.Classes.Api.dll"]

