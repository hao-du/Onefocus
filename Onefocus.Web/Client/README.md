# How to setup

## Steps:

1. Navigate to ..\Onefocus.Web\Client\wallet
1. Run "npm run dev" for development
1. Run "npm run staging" for staging
1. Run "npm run prod" for production

## Technical Points

### vite.config.ts

`html-update-script-tag` and `html-update-link-tag` is to replace incorrect url in `index.html` in `dist` folder.

Only have source map in development mode.