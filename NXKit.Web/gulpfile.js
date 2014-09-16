var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del(['nxkit.js'], cb); 
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit.prefix',
            'nxkit.ts.js',
            'nxkit.suffix'
        ])
        .pipe(concat('nxkit.js'))
        .pipe(gulp.dest('.'))
        .pipe(uglify())
        .pipe(rename(function (path) {
            if (path.extname === '.js') {
                path.basename += '.min';
            }
        }))
        .pipe(gulp.dest('.'));
});

gulp.task('default', ['scripts']);