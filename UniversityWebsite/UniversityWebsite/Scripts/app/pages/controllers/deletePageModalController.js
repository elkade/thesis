angular.module('configApp.pages')

.controller('deletePageModalCtrl', function ($scope, $modalInstance, pages) {
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.ok = function () {
        $modalInstance.close("ok");
    };
})