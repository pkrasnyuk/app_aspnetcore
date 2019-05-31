const path = require("path");
const webpack = require("webpack");
const merge = require("webpack-merge");
const AotPlugin = require("@ngtools/webpack").AotPlugin;
const CheckerPlugin = require("awesome-typescript-loader").CheckerPlugin;
const CopyWebpackPlugin = require("copy-webpack-plugin");

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const sharedConfig = {
        stats: { modules: false },
        context: __dirname,
        resolve: { extensions: [ ".js", ".ts" ] },
        output: {
            filename: "[name].js",
            publicPath: "dist/"
        },
        module: {
            rules: [
                { test: /\.ts$/, include: /ClientApp/, use: ["awesome-typescript-loader?silent=true", "angular2-template-loader"] },
                { test: /\.html$/, use: "html-loader?minimize=false" },
                { test: /\.json$/, use: "file-loader?minimize=false&name=[name].json" },
                { test: /\.css$/, use: [ "to-string-loader", isDevBuild ? "css-loader" : "css-loader?minimize" ] },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: "url-loader?limit=25000" }
            ]
        },
        plugins: [new CheckerPlugin(), new CopyWebpackPlugin([
            { from: "./ClientApp/app/core/data/configuration.json" },
            { from: "./ClientApp/app/public" }
        ],
            {
                copyUnmodified: true
            })]
    };

    const clientBundleOutputDir = "./wwwroot/dist";
    const clientBundleConfig = merge(sharedConfig, {
        entry: { 'main-client': "./ClientApp/boot.browser.ts" },
        output: { path: path.join(__dirname, clientBundleOutputDir) },
        plugins: [
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require("./wwwroot/dist/vendor-manifest.json")
            })
        ].concat([
            new webpack.SourceMapDevToolPlugin({
                filename: "[file].map",
                moduleFilenameTemplate: path.relative(clientBundleOutputDir, "[resourcePath]")
            })
        ])
    });

    const serverBundleConfig = merge(sharedConfig, {
        resolve: { mainFields: ["main"] },
        entry: { 'main-server': "./ClientApp/boot.server.ts" },
        plugins: [
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require("./ClientApp/dist/vendor-manifest.json"),
                sourceType: "commonjs2",
                name: "./vendor"
            })
        ].concat([]),
        output: {
            libraryTarget: "commonjs",
            path: path.join(__dirname, "./ClientApp/dist")
        },
        target: "node",
        devtool: "inline-source-map"
    });

    return [clientBundleConfig, serverBundleConfig];
};
