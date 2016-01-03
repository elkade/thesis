angular.module('configApp.subjects')

.controller('subjectSectionFormCtrl', function ($scope, $sce, subjectsService, utils) {

    $scope.formHide = true;
    
    $scope.updateSubject = function (subject) {
        console.log($scope.form);
        utils.showValidation($scope.form);
        if ($scope.form.$valid) {
            subjectsService.update({ id: subject.Id }, subject, function (response) {
                $scope.editMode = false;
                $scope.formHide = true;
            }, errorHandler);
        }
    };

    $scope.edit = function () {
        $scope.formHide = false;
    };

    $scope.hideForm = function () {
        $scope.formHide = true;
    };

    var errorHandler = function (response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };
})