var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');

gulp.task('clean', function (cb) {
    del(['nxkit-ng.js'], cb); 
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-ng.prefix',
            './src/js/**/*.js',
            'nxkit-ng.suffix'
        ])
        .pipe(concat('nxkit-ng.js'))
        .pipe(gulp.dest('.'));
});

gulp.task('default', ['scripts']);