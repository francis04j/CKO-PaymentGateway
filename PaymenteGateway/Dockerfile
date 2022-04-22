FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /bld
COPY ["Src/WebApi/WebApi.csproj", "."]
RUN dotnet restore "./WebApi.csproj"
COPY . .
WORKDIR "/bld/."
RUN dotnet build "Src/WebApi/WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Src/WebApi/WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#This allows some time for the SQL Server database image to start up.
COPY ["entrypoint.sh","."]

RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
#ENTRYPOINT ["dotnet", "WebApi.dll"]