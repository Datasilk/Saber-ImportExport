var gulp = require('gulp'),
    del = require('del'),
    sevenBin = require('7zip-bin'),
    sevenZip = require('node-7z');

var app = 'ImportExport';
var release = 'bin/Release/net5.0/';
var publish = 'bin/Publish/';

function publishToPlatform(platform) {
    return gulp.src([
        //include custom resources
        'export.html', 'import.html', 'importexport.js',
        //include all files from published folder
        release + platform + app + '/publish/*'
    ]).pipe(gulp.dest(publish + app + '/' + platform, { overwrite: true }));
}

gulp.task('publish:win-x64', () => {
    return publishToPlatform('win-x64');
});

gulp.task('publish:linux-x64', () => {
    return publishToPlatform('linux-x64');
});

gulp.task('zip', () => {
    process.chdir(publish);
    sevenZip.add(app + '.7z', 'ImportExport', {
        $bin: sevenBin.path7za,
        recursive: true
    });
    process.chdir('../..');
    del('bin/Release', { force: true });
    return gulp.src('.');
});

gulp.task('publish', gulp.series('publish:win-x64', 'publish:linux-x64', 'zip'));