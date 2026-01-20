#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG venv=Production
ENV ASPNETCORE_ENVIRONMENT=${venv}

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/services/Ibr.IBPT/Ibr.IBPT.csproj", "src/services/Ibr.IBPT/"]
COPY ["src/building blocks/Ibr.Core/Ibr.Core.csproj", "src/building blocks/Ibr.Core/"]
RUN dotnet restore "src/services/Ibr.IBPT/Ibr.IBPT.csproj"
COPY . .
WORKDIR "/src/src/services/Ibr.IBPT"
RUN dotnet build "Ibr.IBPT.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ibr.IBPT.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ibr.IBPT.dll"]