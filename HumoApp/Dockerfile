FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HumoApp/HumoApp.csproj", "HumoApp/"]
RUN dotnet restore "HumoApp/HumoApp.csproj"
COPY HumoApp/. HumoApp/
RUN dotnet publish "HumoApp/HumoApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "HumoApp.dll"]