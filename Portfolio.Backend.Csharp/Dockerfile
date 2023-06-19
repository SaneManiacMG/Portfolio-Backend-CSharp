# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
# select current directory as working directory
WORKDIR /app/Portfolio.Backend.Csharp
# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore
# copy source files to image directory
COPY . ./
RUN dotnet publish -c Release -o /publish

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "WebApplication.dll"]
