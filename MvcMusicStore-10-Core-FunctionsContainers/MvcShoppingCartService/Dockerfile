FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

# dotnet commands to build, test, and publish
FROM microsoft/dotnet:2.1-sdk AS build
ARG csproj="MvcShoppingCartService.csproj"
WORKDIR /src
COPY . ./
RUN dotnet restore "MvcShoppingCartService/$csproj"
WORKDIR "/src/MvcShoppingCartService"
RUN dotnet build "$csproj" -c Release -o /app

FROM build AS publish
ARG csproj="MvcShoppingCartService.csproj"
RUN dotnet publish "$csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MvcShoppingCartService.dll"]