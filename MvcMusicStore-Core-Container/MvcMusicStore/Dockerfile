FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MvcMusicStore/MvcMusicStore.csproj", "MvcMusicStore/"]
RUN dotnet restore "MvcMusicStore/MvcMusicStore.csproj"
COPY . .
WORKDIR "/src/MvcMusicStore"
RUN dotnet build "MvcMusicStore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MvcMusicStore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MvcMusicStore.dll"]