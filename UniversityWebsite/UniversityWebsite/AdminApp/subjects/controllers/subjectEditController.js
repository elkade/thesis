angular.module('configApp.subjects')

.controller('subjectsEditCtrl', function ($scope, $stateParams, utils, subjectsService) {
    $scope.editMode = false;
    if ($stateParams.subjectName == "newSubject") {
        $scope.editMode = true;
    }
    $scope.subject = utils.findByName($scope.subjects, $stateParams.subjectName);

    //$scope.oneAtATime = true;

    $scope.tinymceOptions = {
        lplugins: 'textcolor link code',
        toolbar: "undo redo styleselect bold italic forecolor backcolor code",
        inline: true,
        menu: { // this is the complete default configuration
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link media | template hr' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        }
    };

    $scope.edit = function() {
        $scope.editMode = true;
        //TODO:
    };

    $scope.update = function () {
        utils.showValidation($scope.subjectForm);
        if ($scope.subjectForm.$valid) {
            subjectsService.post($scope.subject, function (response) {
                $scope.subject = response;
                $scope.subjects.push($scope.subject);
            }, errorHandler);
            $scope.editMode = false;
        }

    };

    var errorHandler = function (response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };
})