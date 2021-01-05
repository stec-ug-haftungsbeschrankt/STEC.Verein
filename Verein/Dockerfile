FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /app

# copy everything and restore as distinct layers
#COPY *.sln .
COPY *.csproj ./verein/
COPY . ./verein/
WORKDIR /app/verein
RUN dotnet restore

# build app
RUN dotnet publish -c Release -o out
CMD []

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS runtime
# Set locale
#RUN apk update
#RUN apk add tzdata
#RUN cp /usr/share/zoneinfo/Europe/Berlin /etc/localtime
#RUN echo "Europe/Berlin" > /etc/timezone
#RUN date

WORKDIR /app
COPY --from=build /app/verein/out ./
ENTRYPOINT ["dotnet", "Verein.dll"]
#ENTRYPOINT bash
#CMD ["dotnet", "Verein.dll"]