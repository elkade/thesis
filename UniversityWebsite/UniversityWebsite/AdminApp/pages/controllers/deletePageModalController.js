angular.module('configApp.pages')

.controller('deletePageModalCtrl', function ($scope, $modalInstance, $location, pagesService, page) {
    $scope.title = "Czy na pewno chcesz usunąć tę stronę?";
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.ok = function () {
        pagesService.remove({ id: page.Id });
        $modalInstance.close("ok");
        $location.path('pages');
    };
    
})