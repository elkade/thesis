angular.module('configApp.pages')

.controller('pagesEditCtrl', function ($scope, $stateParams, utils, post, $modal) {
    $scope.update = function () {
        console.log($scope.page);
        var pos = new post($scope.page);
        pos.$save(function (response) {
            $scope.state = response.$resolved ? 'success' : 'error';
        });
    }

    //$scope.page = utils.findByName($scope.pages, $stateParams.pageName);
        console.log($scope.page);

    $scope.tinymceOptions = {
        height: 500,
        plugins: 'textcolor link code',
        toolbar: "undo redo styleselect bold italic forecolor backcolor code",
        menu: { // this is the complete default configuration
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link media | template hr' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        }
    };

    $scope.open = function () {

        var modalInstance = $modal.open({
            templateUrl: 'scripts/app/views/pages/deletePageModal.html',
            controller: 'deletePageModalCtrl'
        });

    };


})