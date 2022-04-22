#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG VERSION=5.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:${VERSION} AS build
WORKDIR /app
COPY . .

WORKDIR "/app/Src/WebApi"
RUN dotnet restore "WebApi.csproj" -r linux-musl-x64


FROM build AS publish
RUN dotnet publish -c Release -o /out -r linux-musl-x64 --self-contained=true --no-restore -p:PublishReadyToRun=true -p:PublishTrimmed=true

# Final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:${VERSION}
WORKDIR /app

COPY --from=publish /out .


RUN apk add libgdiplus --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted && \
    apk add terminus-font && \
    apk add --no-cache icu-libs
# https://www.abhith.net/blog/docker-sql-error-on-aspnet-core-alpine/
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["./WebApi"]