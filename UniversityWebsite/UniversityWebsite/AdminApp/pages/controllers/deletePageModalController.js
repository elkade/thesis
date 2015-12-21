angular.module('configApp.pages')

.controller('deletePageModalCtrl', function ($scope, $modalInstance, $location, pagesService, page, pages, utils) {
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.ok = function () {
        pagesService.remove({ id: page.Id });
        utils.removeByTitle(pages, page.Title);
        $modalInstance.close("ok");
        $location.path('pages');
    };
    
})