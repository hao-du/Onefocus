# React + TypeScript + Vite

## Technical Points

### vite.config.ts

`html-update-script-tag` and `html-update-link-tag` is to replace incorrect url in `index.html` in `dist` folder.

Only have source map in development mode.

### Connect to DB:
ssh -i "D:\jumpbox\onefocus-vm-jumpbox_key.pem" -N -L 5432:onefocus-db.postgres.database.azure.com:5432 azureadministrator@20.212.240.244
