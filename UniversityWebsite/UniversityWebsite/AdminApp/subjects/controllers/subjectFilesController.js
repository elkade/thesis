angular.module('configApp.subjects')

.controller('subjectFilesCtrl', function ($scope, filesService, utils, $timeout) {
    $scope.totalFiles = 50;
    $scope.currentPage = 1;
    $scope.filesPerPage = 8;

    getPage(1);

    $scope.pageChanged = function (newPage) {
        getPage(newPage);
    };

    $scope.pagination = {
        current: 1
    };

    $scope.uploadFiles = function (file, errFiles) {
        $scope.file = file;
        $scope.errFile = errFiles && errFiles[0];
        if (file) {
            file.upload = filesService.upload($scope.subject.Id, file);

            file.upload.then(function (response) {
                $timeout(function () {
                    $scope.files.push(response.data);
                });
            }, function (response) {
                if (response.status > 0)
                    $scope.errorMsg = response.status + ': ' + response.data;
            }, function (evt) {
                file.progress = Math.min(100, parseInt(100.0 *
                    evt.loaded / evt.total));
            });
        }
    };

    $scope.removeFiles = function () {
        var selectedFiles = Enumerable.From($scope.files).Where(function (file) { return file.selected; }).ToArray();
        Enumerable.From(selectedFiles).ForEach(function (file) {
            filesService.remove({ fileId: file.Id });
            utils.remove($scope.files, file);
        });
        getPage(1);
    };

    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.filesPerPage;
        filesService.allFiles({ subjectId: $scope.subject.Id, limit: $scope.filesPerPage, offset: offset }, function (files) {
            $scope.files = files;
        });
    };
});