angular.module('configApp.subjects')

.controller('subjectsEditCtrl', function ($scope, $stateParams, utils, subjectsService, filesService, Upload, $timeout) {
    $scope.editMode = false;
    if ($stateParams.subjectName == "newSubject") {
        $scope.editMode = true;
    }
    $scope.subject = utils.findByName($scope.subjects, $stateParams.subjectName);
    console.log($scope.subject);
    //$scope.oneAtATime = true;

    $scope.tinymceOptions = {
        lplugins: 'textcolor link code',
        toolbar: "undo redo styleselect bold italic forecolor backcolor code",
        menu: { // this is the complete default configuration
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link media | template hr' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        },

        setup: function (ed) {
        },
    };

    $scope.edit = function() {
        $scope.editMode = true;
        //TODO:
    };

    $scope.update = function () {
        utils.showValidation($scope.subjectForm);
        if ($scope.subjectForm.$valid) {
            if ($scope.subject.Id == null) {
                $scope.subject.Syllabus = new Object();
                $scope.subject.Schedule = new Object();
                subjectsService.post($scope.subject, function(response) {
                    $scope.subject = response;
                    $scope.subjects.push($scope.subject);
                    $scope.editMode = false;
                }, errorHandler);
            } else {
                subjectsService.update({ id: $scope.subject.Id }, $scope.subject, function (response) {
                    $scope.subject = response;
                    $scope.editMode = false;
                }, errorHandler);
            }
        }

    };

    $scope.createNews = function () {
        var news = new Object();
        $scope.subject.News.push(news);
    };

    var errorHandler = function (response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };

    $scope.loadFiles = function () {
        if ($scope.files == null) {
            filesService.allFiles({ subjectId: $scope.subject.Id }, function (files) {
                $scope.files = files;
                console.log("OK");
            });
        }
    };

    //TODO: move to service
    $scope.uploadFiles = function(file, errFiles) {
        $scope.f = file;
        $scope.errFile = errFiles && errFiles[0];
        if (file) {
            file.upload = Upload.upload({
                url: 'api/file?subjectId=' + $scope.subject.Id,
                data: { file: file }
            });

            file.upload.then(function(response) {
                $timeout(function() {
                    $scope.files.push(response.data);
                });
            }, function(response) {
                if (response.status > 0)
                    $scope.errorMsg = response.status + ': ' + response.data;
            }, function(evt) {
                file.progress = Math.min(100, parseInt(100.0 *
                    evt.loaded / evt.total));
            });
        }
    };

    $scope.removeFiles = function() {
        var selectedFiles = Enumerable.From($scope.files).Where(function(file) { return file.selected; }).ToArray();
        Enumerable.From(selectedFiles).ForEach(function (file) {
            filesService.remove({ fileId: file.Id });
            utils.remove($scope.files, file);
        });
    };
})