var gulp = require('gulp');
var util = require('gulp-util');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var less = require('gulp-less');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        util.env.TargetDir + 'nxkit.js',
        util.env.TargetDir + 'nxkit.min.js',
        util.env.TargetDir + 'nxkit.html',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit.js.prefix',
            'nxkit.ts.js',
            'nxkit.js.suffix',
        ])
        .pipe(concat('nxkit.js'))
        .pipe(gulp.dest(util.env.TargetDir))
        .pipe(uglify())
        .pipe(rename('nxkit.min.js'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src([
            '*.html',
        ])
        .pipe(concat('nxkit.html'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('less', ['clean'], function () {
    return gulp.src([
            '*.less',
    ])
        .pipe(less())
        .pipe(concat('nxkit.css'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('default', ['scripts', 'templates', 'less']);