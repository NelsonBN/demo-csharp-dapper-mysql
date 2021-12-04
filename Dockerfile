#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.0 AS base-env
WORKDIR /app
EXPOSE 80



# BUILD APPLICATION
FROM mcr.microsoft.com/dotnet/sdk:6.0.100 AS build-env

WORKDIR /src

# Copy project to restore nugets
COPY ./src/*.csproj .

# Restore nugets
RUN dotnet restore *.csproj



# Copy all source code to build
COPY ./src .
RUN dotnet build *.csproj -c Release -o /app/build


# Publish application. Used the "--no-restore" to benefit the layer caches
FROM build-env AS publish-env
RUN dotnet publish *.csproj -c Release --no-restore -o /app/publish



FROM base-env AS final-env
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Production

COPY --from=publish-env /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.Dapper.Mysql.dll"]