var gulp = require('gulp');
var util = require('gulp-util');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        util.env.TargetDir + 'nxkit-xforms-layout.js',
        util.env.TargetDir + 'nxkit-xforms-layout.min.js',
        util.env.TargetDir + 'nxkit-xforms-layout.html',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-xforms-layout.js.prefix',
            'nxkit-xforms-layout.ts.js',
            'nxkit-xforms-layout.js.suffix',
        ])
        .pipe(concat('nxkit-xforms-layout.js'))
        .pipe(gulp.dest(util.env.TargetDir))
        //.pipe(uglify())
        .pipe(rename('nxkit-xforms-layout.min.js'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src([
        '*.html',
        ])
        .pipe(concat('nxkit-xforms-layout.html'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('default', ['scripts', 'templates']);
