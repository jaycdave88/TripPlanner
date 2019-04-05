FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY TripTrackerLive.sln ./
COPY TripTracker.BackService/TripTracker.BackService.csproj TripTracker.BackService/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/TripTracker.BackService
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

RUN apt-get update && apt-get install -y bash curl

RUN mkdir -p /opt/datadog
RUN curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v1.0.0/datadog-dotnet-apm_1.0.0_amd64.deb
RUN apt install ./datadog-dotnet-apm_1.0.0_amd64.deb

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json

ENTRYPOINT ["dotnet", "TripTracker.BackService.dll"]
