var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        'nxkit.js',
        'nxkit.min.js',
        'nxkit.html',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit.prefix',
            'nxkit.ts.js',
            'nxkit.suffix',
        ])
        .pipe(concat('nxkit.js'))
        .pipe(gulp.dest('.'))
        .pipe(uglify())
        .pipe(rename('nxkit.min.js'))
        .pipe(gulp.dest('.'));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src([
            '*.html',
        ])
        .pipe(concat('nxkit.html'))
        .pipe(gulp.dest('.'));
});

gulp.task('default', ['scripts', 'templates']);