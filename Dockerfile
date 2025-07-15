# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY *.csproj .
COPY mobi_test_test_project/*.sln ./mobi_test_test_project/
COPY mobi_test_test_project/*.csproj ./mobi_test_test_project/
RUN dotnet restore

# copy everything else and build app
COPY . ./internship-entry-task-tic-tac-toe-game/
WORKDIR /source/internship-entry-task-tic-tac-toe-game
RUN dotnet build mobibank_test.sln -c Release -o /app
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "mobibank_test.dll"]