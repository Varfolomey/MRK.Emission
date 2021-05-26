#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MRK.Emission.Api/MRK.Emission.Api.csproj", "MRK.Emission.Api/"]
RUN dotnet restore "MRK.Emission.Api/MRK.Emission.Api.csproj"
COPY . .
WORKDIR "/src/MRK.Emission.Api"
RUN dotnet build "MRK.Emission.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MRK.Emission.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MRK.Emission.Api.dll"]