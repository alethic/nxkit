var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');

gulp.task('clean', function (cb) {
    del(['nxkit-xforms.js'], cb); 
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-xforms.prefix',
            'nxkit-xforms.ts.js',
            'nxkit-xforms.suffix'
        ])
        .pipe(concat('nxkit-xforms.js'))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('default', ['scripts']);