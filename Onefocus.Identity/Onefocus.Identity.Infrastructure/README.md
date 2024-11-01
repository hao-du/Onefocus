# Onefocus.Identity.Infrastructure
## Overview
This is infrastructure layer in CQRS architecture for Identity app. 

## Technical Points
### Add EF migration cs file
Under *Onefocus.Identity.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations add <from-this-migration> <to-this-migration> --context IdentityDbContext -o "Databases/Migrations" --startup-project ../Onefocus.Identity.Api`

### Create EF migration SQL script
Under *Onefocus.Identity.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations script <from-this-migration> <to-this-migration> --startup-project '../Onefocus.Identity.Api' -o script.sql`

or to generate entire sql script:

`dotnet ef migrations script --startup-project '../Onefocus.Identity.Api' -o script.sql`