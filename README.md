
# Onefocus

## How to run:
There are 2 options to run this project:

### 1. Use docker:
**Impacted files:**
| File | Description |
|--|--|
| \Onefocus\docker-compose.yml | To setup all docker images. <br> *Note: Ports: 4000:8080 means 4000 is mapping to localhost, 8080 is docker internal port* |
|\Onefocus\docker-compose.override.yml|To override dev environment and port|
|appsettings.Docker.json|Each webapp/api will have this appsetting file to store dev settings|
|\Onefocus\Onefocus.Gateway\appsettings.Docker.json|Store docker internal endpoint e.g. membership, identity etc... These names come from service names in **docker-compose.yml** |
|Dockerfile|Each webapp/api will have this file. To add this file, right click on project --> add --> Docker Support|
To run docker, select docker-compose --> start Docker Compose

### 2. Use .Net Aspire:
**Impacted files:**
| File | Description |
|--|--|
|\Onefocus\Onefocus.AppHost\Program.cs|To add all webapp/api. Select project --> Add --> .NET Aspire Orchestrator Support|
|appsettings.Development.json|Each webapp/api will have this appsetting file to store dev settings|

To run aspire, select Onefocus.AppHost --> start https