angular.module('configApp.pages')

.controller('deletePageModalCtrl', function ($scope, $modalInstance, Pages, page, pages, utils) {
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.ok = function () {
        Pages.remove({ id: page.Title });
        utils.removeByTitle(pages, page.Title);
        $modalInstance.close("ok");
    };
    
})