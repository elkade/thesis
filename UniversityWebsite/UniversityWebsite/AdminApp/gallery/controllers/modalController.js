angular.module('configApp.gallery')


.controller('modalCtrl', function ($scope, $modalInstance) {
    $scope.ok = function () {
        $modalInstance.close($scope.selected.item);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.reposition = function () {
        $modalInstance.reposition();
    };

    $scope.selectImage = function(imageUrl) {
        $modalInstance.close(imageUrl);
    };
})