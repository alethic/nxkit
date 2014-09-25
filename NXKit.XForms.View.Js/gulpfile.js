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
        util.env.IntermediateOutputPath + 'nxkit-xforms.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms.min.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms.html',
        util.env.IntermediateOutputPath + 'nxkit-xforms.css',
        util.env.OutputPath + 'nxkit-xforms.js',
        util.env.OutputPath + 'nxkit-xforms.min.js',
        util.env.OutputPath + 'nxkit-xforms.html',
        util.env.OutputPath + 'nxkit-xforms.css',
    ], cb);
});

gulp.task('scripts', [], function () {
    return gulp.src([
            'nxkit-xforms.js.prefix',
            util.env.IntermediateOutputPath + 'nxkit-xforms.ts.js',
            'nxkit-xforms.js.suffix',
    ])
        .pipe(concat('nxkit-xforms.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath))
        .pipe(uglify())
        .pipe(rename('nxkit-xforms.min.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('templates', [], function () {
    return gulp.src([
            '*.html',
    ])
        .pipe(concat('nxkit-xforms.html'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('less', [], function () {
    return gulp.src([
            '*.less',
    ])
        .pipe(less())
        .pipe(concat('nxkit-xforms.css'))
        .pipe(css())
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('compile', ['scripts', 'templates', 'less']);

gulp.task('build', ['compile'], function () {
    return gulp.src([
        util.env.IntermediateOutputPath + 'nxkit-xforms.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms.min.js',
        util.env.IntermediateOutputPath + 'nxkit-xforms.html',
        util.env.IntermediateOutputPath + 'nxkit-xforms.css',
    ])
        .pipe(gulp.dest(util.env.OutputPath));
});

gulp.task('default', ['scripts', 'templates', 'less', 'build']);
