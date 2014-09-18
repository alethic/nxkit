var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        'nxkit-xforms.js',
        'nxkit-xforms.min.js',
        'nxkit-xforms.html',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-xforms.prefix',
            'nxkit-xforms.ts.js',
            'nxkit-xforms.suffix',
        ])
        .pipe(concat('nxkit-xforms.js'))
        .pipe(gulp.dest('.'))
        //.pipe(uglify())
        .pipe(rename('nxkit-xforms.min.js'))
        .pipe(gulp.dest('.'));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src([
            '*.html',
        ])
        .pipe(concat('nxkit-xforms.html'))
        .pipe(gulp.dest('.'));
});

gulp.task('default', ['scripts', 'templates']);
