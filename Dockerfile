﻿FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
COPY ["ScheduleSender/Images", "root/.net/Images"]
COPY ["ScheduleSender/Fonts", "root/.net/Fonts"]
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ScheduleSender/ScheduleSender.csproj", "ScheduleSender/"]
RUN dotnet restore "ScheduleSender/ScheduleSender.csproj"
COPY . .
WORKDIR "/src/ScheduleSender"
RUN dotnet build "ScheduleSender.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduleSender.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./ScheduleSender"]
