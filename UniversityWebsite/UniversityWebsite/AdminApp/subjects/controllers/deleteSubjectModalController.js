angular.module('configApp.subjects')

.controller('deleteSubjectModalCtrl', function ($scope, $modalInstance, $state, subjectsService, subject, subjects, utils) {
    $scope.title = "Czy na pewno chcesz usunąć ten przedmiot?";
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.ok = function () {
        subjectsService.removeSubject(subject.Id).then(function (resp) {
            $modalInstance.close("ok");
            utils.removeByName(subjects, subject.Name);
            $state.go('subjects');
        }, function(error) {
            $modalInstance.dismiss(error);
        });
    };

})