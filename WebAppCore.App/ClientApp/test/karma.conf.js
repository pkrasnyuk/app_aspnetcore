module.exports = function (config) {
    config.set({
        basePath: ".",
        frameworks: ["jasmine"],
        files: [
            "../../wwwroot/dist/vendor.js",
            "./boot-tests.ts"
        ],
        preprocessors: {
            './boot-tests.ts': ["webpack"]
        },
        reporters: ["progress"],
        port: 9876,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ["Chrome"],
        mime: { 'application/javascript': ["ts","tsx"] },
        singleRun: false,
        webpack: require("../../webpack.config.js")().filter(config => config.target !== "node"),
        webpackMiddleware: { stats: "errors-only" }
    });
};
