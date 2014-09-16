var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');

gulp.task('clean', function (cb) {
    del(['nxkit-xforms-layout.js'], cb); 
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-xforms-layout.prefix',
            'nxkit-xforms-layout.ts.js',
            'nxkit-xforms-layout.suffix'
        ])
        .pipe(concat('nxkit-xforms-layout.js'))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('default', ['scripts']);