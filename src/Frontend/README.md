# Frontend

## Assets (ts / scss / fonts)
### Build assets in production mode
to build the assets navigate to the `Assets` folder.
run the following commands for production:

```bash
npm i
npm run build
```

this should generate the files in `wwwroot/css`,`wwwroot/fonts`,`wwwroot/js`

### Watch files for development
Navigate to the `Assets` folder and run the following command:

```bash
npm i
npm run dev
```

this will start the webpack watcher which recompiles as soon as a file changes