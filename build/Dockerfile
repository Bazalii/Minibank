FROM mcr.microsoft.com/dotnet/sdk:6.0 AS src
WORKDIR /src
COPY . .

WORKDIR src/
RUN dotnet build MiniBank.Web -c Release -r linux-x64

RUN dotnet test Tests/MiniBank.Core.Tests --no-build 

RUN dotnet publish MiniBank.Web -c Release -r linux-x64 --no-build -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

WORKDIR /app

COPY --from=src /dist .

EXPOSE 5000 5001
ENV ASPNETCORE_URLS=http://localhost:5000;http://localhost:5001

ENTRYPOINT ["dotnet", "MiniBank.Web.dll"]