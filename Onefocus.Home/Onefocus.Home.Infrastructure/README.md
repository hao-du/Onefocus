# Onefocus.Home.Infrastructure
## Overview
This is infrastructure layer in CQRS architecture for Home app. 

## Technical Points
### Add EF migration cs file
Under *Onefocus.Homne.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations add <from-this-migration> <to-this-migration> --context HomeWriteDbContext -o "Databases/Migrations" --startup-project ../Onefocus.Home.Api`

### Create EF migration SQL script
Under *Onefocus.Membership.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations script <from-this-migration> <to-this-migration> --startup-project '../Onefocus.Home.Api' -o script.sql`

or to generate entire sql script:

`dotnet ef migrations script --startup-project '../Onefocus.Home.Api' --context HomeWriteDbContext -o script.sql`