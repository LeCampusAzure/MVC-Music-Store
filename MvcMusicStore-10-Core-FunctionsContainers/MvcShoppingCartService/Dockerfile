FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

# dotnet commands to build, test, and publish
FROM microsoft/dotnet:2.1-sdk AS build
RUN ls -R
WORKDIR /src
RUN ls -R
COPY . ./
RUN ls -R
WORKDIR "/src/MvcShoppingCartService"
RUN dotnet restore "MvcShoppingCartService.csproj"
RUN dotnet build "MvcShoppingCartService.csproj" -c Release
RUN dotnet publish "MvcShoppingCartService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MvcShoppingCartService.dll"]