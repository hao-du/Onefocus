# Onefocus.Wallet.Infrastructure
## Overview
This is infrastructure layer in CQRS architecture for Identity app. 

## Technical Points
### Add EF migration cs file
Under *Onefocus.Search.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations add <name> --context SearchDbContext -o "Databases/Migrations" --startup-project ../Onefocus.Search.Api`

### Create EF migration SQL script
Under *Onefocus.Wallet.Infrastructure* folder, run this command in **Powershell**:

`dotnet ef migrations script <from-this-migration> <to-this-migration> --startup-project '../Onefocus.Search.Api' --context SearchDbContext -o script.sql`

or to generate entire sql script:

`dotnet ef migrations script --startup-project '../Onefocus.Search.Api' --context SearchDbContext -o script.sql`

e.g.:
`dotnet ef migrations script 20250614163727_CreateSchema 20250707153531_UpdateTransactionCurrencyConstraint --startup-project '../Onefocus.Search.Api' --context SearchDbContext -o script.sql`