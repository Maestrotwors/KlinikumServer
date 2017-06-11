/// <binding ProjectOpened='start' />
"use strict";

var gulp = require("gulp"),
    
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    watch = require('gulp-watch'),
    pagebuilder = require('gulp-pagebuilder'),
    fs = require('fs');

var paths = {
    webroot: "./wwwroot/"
};

paths.js_Main = "./files/js/files/main/*.js";
paths.css_Main = "./files/css/main/*.css";
paths.js_Library = "./files/js/library/*.js";
paths.js_LibraryFirst = "./files/js/library/first/*.js";
paths.css_Library = "./files/css/library/*.css";
paths.concatJsDest_Main = paths.webroot + "min/main.min.js";
paths.concatCssDest_Main = paths.webroot + "min/main.min.css";
paths.concatJsDest_Library = paths.webroot + "min/library.min.js";
paths.concatCssDest_Library = paths.webroot + "min/library.min.css";

gulp.task("min:js_Main", function () {
    return gulp.src([
        "./files/js/main/GlobalData.js",
        "./files/js/main/GlobalMethods.js",
        "./files/js/main/Router.js",
        "./files/js/main/Socket.js"
        ])
        .pipe(concat(paths.concatJsDest_Main))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

 
gulp.task('SaveUpdateTime', function () {
    var tm = new Date();
    var resTxt = '';

    resTxt += tm.getDate() + "." + (tm.getMonth() + 1)
        + "." + tm.getFullYear()+"  "; 

    resTxt += tm.getHours() + ":"
        + tm.getMinutes() + ":" + tm.getSeconds();

    fs.writeFileSync('UpdateTime.txt', resTxt );
}); 

gulp.task("min:css_Main", function () {
    return gulp.src([paths.css_Main])
        .pipe(concat(paths.concatCssDest_Main))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min:js_Library", function () {
    return gulp.src([paths.js_LibraryFirst, paths.js_Library], { base: "." })
        .pipe(concat(paths.concatJsDest_Library))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css_Library", function () {
    return gulp.src([paths.css_Library])
        .pipe(concat(paths.concatCssDest_Library))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task('make_main.html', function () {
    gulp.src('./files/templates/main/main.html')
        .pipe(pagebuilder('Main'))
        .pipe(gulp.dest('./build/'));
});


 
gulp.task("start", function () { 
    gulp.watch(paths.js_Main, ['min:js_Main']);
    gulp.watch(paths.css_Main, ['min:css_Main']);
    gulp.watch(paths.js_Library, ['min:js_Library']);
    gulp.watch(paths.css_Library, ['min:css_Library']);
    gulp.watch(['./files/**', './Views/**', './Controllers/**'], ['SaveUpdateTime']);
});

gulp.task("minimize_files_js_and_css", ["min:js_Main", "min:css_Main"]);
gulp.task("minimize_library", ["min:js_Library", "min:css_Library"]);
gulp.task("minimize_all", ["minimize_files_js_and_css", "minimize_library"]);