##### [Home repo](https://github.com/overwasher/home/) | [Overwatcher code](https://github.com/overwasher/overwatcher) | [Sensor Node code](https://github.com/overwasher/esp-firmware) | [Telegram bot code](https://github.com/overwasher/telegram-bot) | [Task Tracker](https://taiga.dcnick3.me/project/overwasher/)

Overwatcher component - receives, aggregates and archives sensor node data and provides a nice API to query the current states.

This is a C# wewrite of legacy TypeScript version, created to allow for more testability and maintainability.

## Software Stack

We use asp.net core web framework (in Web API configuration specifically). InfluxDB is also used to store historical sensor node state values, as well as raw telemetry.

The architecture is not object oriented per se, but more service oriented, as dictated by asp.net core dependency injection framework. (see [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) for more info about dependency injection in asp.net core)

## How to build & run

First, you would need to setup an InfluxDB instance used for telemetry storage. You should put [connection string](https://github.com/influxdata/influxdb-client-csharp/tree/master/Client#client-connection-string) into [asp.net core configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration) under key `SensorInfluxDB` (for example in `appsettings.Development.json`)

Then you can proceed with building and running:

```bash
dotnet run
```

## Api Documentation

The Api documentation is auto-generated by swagger. The production docs can be found [here](https://overwatcher.ow.dcnick3.me/swagger/index.html).

## How to contribute

Currently, the project is in heavy development and may change a lot in the nearest future. 

*We do not recommend contributing at this stage*. 

Later, when project becomes more stable, we may create some contribution guidelines for everyone to use. 
