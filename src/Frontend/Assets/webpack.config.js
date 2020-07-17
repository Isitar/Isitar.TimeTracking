const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

//backend
const backendConfig = (env) => {
    const mode = env.mode || 'production';
    const modeDev = mode !== 'production';
    const cleanOptions = {
        root: __dirname + '/../wwwroot',
        verbose: true,
        dry: false,
        cleanOnceBeforeBuildPatterns: [
            "js/*.js",
            "css/*.css",
            "fonts/*.eot",
            "fonts/*.ttf",
            "fonts/*.woff",
            "fonts/*.woff2",
            "fonts/*.svg"
        ]
    };
    return {
        mode: mode,
        watch: modeDev,
        entry: {
            site: [
                "./ts/site.ts"
            ],
            icons: [
                "./ts/icons.ts"
            ]
        },
        output: {
            path: __dirname + "/../wwwroot",
            filename: "js/[name].min.js",
        },
        resolve: {
            //add '.ts' and '.tsx' as resolvable extensions.
            extensions: [".ts", ".tsx", ".js", ".json"]
        },
        module: {
            rules: [
                {
                    //all files with a '.ts' or '.tsx' extension will be handled by 'awesome-typescript-loader'.
                    test: /\.tsx?$/,
                    loader: "awesome-typescript-loader",
                    options: {
                        configFileName: "tsconfig.json"
                    }
                },
                {
                    test: /\.(sa|sc|c)ss$/,
                    use: [
                        {
                            loader: MiniCssExtractPlugin.loader
                        },
                        {
                            loader: "css-loader",
                            options: {
                                url: false,
                                import: false
                            }
                        },
                        "sass-loader"
                    ],
                },
            ],
        },
        plugins: [
            new MiniCssExtractPlugin({
                filename: "css/[name].min.css"
            }),
            new CleanWebpackPlugin(cleanOptions),
            new CopyWebpackPlugin({
                patterns: [
                    {
                        from: __dirname + "/fonts",
                        to: __dirname + "/../wwwroot/fonts"
                    },
                    {
                        from: __dirname + "/node_modules/@fortawesome/fontawesome-free/webfonts",
                        to: __dirname + "/../wwwroot/fonts"
                    }
                ]
            }),
        ],
        optimization: {
            minimize: !modeDev,
            minimizer: [
                new UglifyJsPlugin({
                    cache: modeDev,
                    parallel: true,
                    sourceMap: modeDev,
                    extractComments: !modeDev,
                    uglifyOptions: {}
                }),
                new OptimizeCSSAssetsPlugin({
                    cssProcessor: require('cssnano'),
                    cssProcessorPluginOptions: {
                        preset: [
                            'default',
                            {
                                discardComments: {
                                    removeAll: !modeDev
                                }
                            }
                        ]
                    }
                })
            ],
        }
    }
};

//actual export of configs
module.exports = [backendConfig];