var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        'nxkit-xforms-layout.js',
        'nxkit-xforms-layout.min.js',
        'nxkit-xforms-layout.html',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-xforms-layout.prefix',
            'nxkit-xforms-layout.ts.js',
            'nxkit-xforms-layout.suffix',
        ])
        .pipe(concat('nxkit-xforms-layout.js'))
        .pipe(gulp.dest('.'))
        //.pipe(uglify())
        .pipe(rename('nxkit-xforms-layout.min.js'))
        .pipe(gulp.dest('.'));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src([
        '*.html',
        ])
        .pipe(concat('nxkit-xforms-layout.html'))
        .pipe(gulp.dest('.'));
});

gulp.task('default', ['scripts', 'templates']);
