FROM karlpothast/pymage.io:1.0.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY https/pymage_io.pfx /https/pymage_io.pfx
COPY src/API/extract-text.sh extract-text.sh  
ENV ENV=/root/.ashrc
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV PYTHONUNBUFFERED=1
RUN apk add --update --no-cache python3 && ln -sf python3 /usr/bin/python
RUN python3 -m ensurepip
RUN pip3 install --no-cache --upgrade pip setuptools 
RUN pip install boto3
RUN apk add coreutils

FROM karlpothast/pymage.io:1.0.0 AS build
WORKDIR /src
COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY . .
WORKDIR /src/src/API

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .



