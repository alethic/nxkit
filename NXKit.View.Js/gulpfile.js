var gulp = require('gulp');
var util = require('gulp-util');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var less = require('gulp-less');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        util.env.IntermediateOutputPath + 'nxkit.js',
        util.env.IntermediateOutputPath + 'nxkit.min.js',
        util.env.IntermediateOutputPath + 'nxkit.html',
        util.env.IntermediateOutputPath + 'nxkit.css',
        util.env.OutputPath + 'nxkit.js',
        util.env.OutputPath + 'nxkit.min.js',
        util.env.OutputPath + 'nxkit.html',
        util.env.OutputPath + 'nxkit.css',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit.js.prefix',
            util.env.IntermediateOutputPath + 'nxkit.ts.js',
            'nxkit.js.suffix',
            'RequireJS/*.js',
        ])
        .pipe(concat('nxkit.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath))
        .pipe(uglify())
        .pipe(rename('nxkit.min.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src([
            '*.html',
        ])
        .pipe(concat('nxkit.html'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('less', ['clean'], function () {
    return gulp.src([
            '*.less',
    ])
        .pipe(less())
        .pipe(concat('nxkit.css'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('compile', ['scripts', 'templates', 'less']);
gulp.task('build', ['compile'], function () {
    return gulp.src([
        util.env.IntermediateOutputPath + 'nxkit.js',
        util.env.IntermediateOutputPath + 'nxkit.min.js',
        util.env.IntermediateOutputPath + 'nxkit.html',
        util.env.IntermediateOutputPath + 'nxkit.css',
    ])
        .pipe(gulp.dest(util.env.OutputPath));
});

gulp.task('default', ['build']);
