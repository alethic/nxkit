var gulp = require('gulp');
var util = require('gulp-util');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var less = require('gulp-less');
var rename = require('gulp-rename');
var css = require('gulp-minify-css');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.min.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.html',
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.css',
        util.env.OutputPath + 'nxkit-xforms-layout.js',
        util.env.OutputPath + 'nxkit-xforms-layout.min.js',
        util.env.OutputPath + 'nxkit-xforms-layout.html',
        util.env.OutputPath + 'nxkit-xforms-layout.css',
    ], cb);
});

gulp.task('scripts', [], function () {
    return gulp.src([
            'nxkit-xforms-layout.js.prefix',
            util.env.IntermediateOutputPath + 'nxkit-xforms-layout.ts.js',
            'nxkit-xforms-layout.js.suffix',
    ])
        .pipe(concat('nxkit-xforms-layout.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath))
        .pipe(uglify())
        .pipe(rename('nxkit-xforms-layout.min.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('templates', [], function () {
    return gulp.src([
            '*.html',
    ])
        .pipe(concat('nxkit-xforms-layout.html'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('less', [], function () {
    return gulp.src([
            '*.less',
    ])
        .pipe(less())
        .pipe(concat('nxkit-xforms-layout.css'))
        .pipe(css())
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('compile', ['scripts', 'templates', 'less']);

gulp.task('build', ['compile'], function () {
    return gulp.src([
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.min.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.html',
        util.env.IntermediateOutputPath + 'nxkit-xforms-layout.css',
    ])
        .pipe(gulp.dest(util.env.OutputPath));
});

gulp.task('default', ['scripts', 'templates', 'less', 'build']);
