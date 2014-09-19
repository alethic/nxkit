var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var util = require('gulp-util');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        util.env.TargetDir + 'nxkit-ng.js',
        util.env.TargetDir + 'nxkit-ng.min.js',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-ng.js.prefix',
            './src/js/**/*.js',
            'nxkit-ng.js.suffix'
        ])
        .pipe(concat('nxkit-ng.js'))
        .pipe(gulp.dest(util.env.TargetDir))
        .pipe(uglify())
        .pipe(rename('nxkit-ng.min.js'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('default', ['scripts']);