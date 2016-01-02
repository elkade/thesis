angular.module('configApp.users')


.controller('usersModalCtrl', function ($scope, $modalInstance) {
    $scope.ok = function () {
        $modalInstance.close($scope.selected.item);
    };
    $scope.userRole = "Teacher";

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.selectUser = function(userId) {
        $modalInstance.close(userId);
    };
})