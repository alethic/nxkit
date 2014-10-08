var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var util = require('gulp-util');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('clean', function (cb) {
    del([
        util.env.IntermediateOutputPath + 'nxkit-ng.js',
        util.env.IntermediateOutputPath + 'nxkit-ng.min.js',
        util.env.OutputPath + 'nxkit-ng.js',
        util.env.OutputPath + 'nxkit-ng.min.js',
    ], cb);
});

gulp.task('scripts', [], function () {
    return gulp.src([
            'nxkit-ng.js.prefix',
            './src/js/**/*.js',
            'nxkit-ng.js.suffix'
        ])
        .pipe(concat('nxkit-ng.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath))
        .pipe(uglify())
        .pipe(rename('nxkit-ng.min.js'))
        .pipe(gulp.dest(util.env.IntermediateOutputPath));
});

gulp.task('compile', ['scripts']);
gulp.task('build', ['compile'], function () {
    return gulp.src([
        util.env.IntermediateOutputPath + 'nxkit-ng.js',
        util.env.IntermediateOutputPath + 'nxkit-ng.min.js',
        util.env.IntermediateOutputPath + 'nxkit-ng.html',
        util.env.IntermediateOutputPath + 'nxkit-ng.css',
    ])
        .pipe(gulp.dest(util.env.OutputPath));
});

gulp.task('default', ['build']);