# Onefocus.Membership.Infrastructure
## Overview
This is infrastructure layer in CQRS architecture for Membership app. 

## Technical Points
### Add EF migration cs file
Under *Onefocus.Membership.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations add <from-this-migration> <to-this-migration> --context IdentityDbContext -o "Databases/Migrations" --startup-project ../Onefocus.Membership.Api`

### Create EF migration SQL script
Under *Onefocus.Membership.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations script <from-this-migration> <to-this-migration> --startup-project '../Onefocus.Membership.Api' -o script.sql`

or to generate entire sql script:

`dotnet ef migrations script --startup-project '../Onefocus.Membership.Api' -o script.sql`