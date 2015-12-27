angular.module('configApp.subjects')

.controller('subjectFilesCtrl', function ($scope, $q, $timeout, filesService) {
    $scope.totalFiles = 10;
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

        console.log(file);
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
        var promises = [];
        var selectedFiles = Enumerable.From($scope.files).Where(function (file) { return file.selected; });

        selectedFiles.ForEach(function (file) {
            var promise = filesService.remove(file.Id);
            promises.push(promise);
        });

        $q.all(promises).then(function () {
            getPage(1);
        });
    };

    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.filesPerPage;
        filesService.queryFiles($scope.subject.Id, $scope.filesPerPage, offset).success(function (resp) {
            $scope.files = resp.Elements;
            $scope.totalFiles = resp.Number;
        }).error(function(error) {
            console.log(error);
        });
    };
});