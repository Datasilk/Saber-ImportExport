var gulp = require('gulp');

var release = 'bin/Release/netcoreapp3.1/';
var publish = 'bin/Publish/ImportExport/'

gulp.task('publish', function () {
    p = gulp.src(['export.html', 'import.html', 'importexport.js', release + 'Saber.Vendor.ImportExport.dll'])
        .pipe(gulp.dest(publish, { overwrite: true }));
    return p;
});