 
"use strict";

var paths = {
    webroot: "./wwwroot/"
};

module.exports = {
    entry: paths.webroot +"input.js",
    output: {
        filename: paths.webroot +"build.js"
    },
    watch: true,
    watchOptions: {
        aggregateTimeOut:100
    },
    devServer: {
        contentBase: ".",
        host: "localhost",
        port: 9000
    },
    module: {
        loaders: [
            {
                test: /\.jsx?$/,
                loader: "babel-loader"
            }
        ]
    }
};