var gulp = require('gulp');
var util= require('gulp-util');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        util.env.TargetDir + 'nxkit-xforms.js',
        util.env.TargetDir + 'nxkit-xforms.min.js',
        util.env.TargetDir + 'nxkit-xforms.html',
    ], cb);
});

gulp.task('scripts', ['clean'], function () {
    return gulp.src([
            'nxkit-xforms.js.prefix',
            'nxkit-xforms.ts.js',
            'nxkit-xforms.js.suffix',
        ])
        .pipe(concat('nxkit-xforms.js'))
        .pipe(gulp.dest(util.env.TargetDir))
        //.pipe(uglify())
        .pipe(rename('nxkit-xforms.min.js'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src([
            '*.html',
        ])
        .pipe(concat('nxkit-xforms.html'))
        .pipe(gulp.dest(util.env.TargetDir));
});

gulp.task('default', ['scripts', 'templates']);
