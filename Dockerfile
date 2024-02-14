# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0.101 AS build
WORKDIR /app

COPY BooksApi.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build -c Release -o out

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish -c Release -o out

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/out ./
ENTRYPOINT ["dotnet", "BooksApi.dll"]