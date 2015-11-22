angular.module('configApp.pages')

.controller('pagesEditCtrl', function ($scope, $stateParams, utils, Pages, $modal) {
    $scope.update = function () {

        if ($scope.page.UrlName != null) {
            Pages.update({ id: $scope.page.Title }, $scope.page, function(response) {
                $scope.state = response.$resolved ? 'success' : 'error';
            });
        } else {
            Pages.post($scope.page, function (response) {
                $scope.state = response.$resolved ? 'success' : 'error';
                $scope.pages.push($scope.page);
            });
        }
    }

    $scope.page = utils.findByTitle($scope.pages, $stateParams.pageName);

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
            controller: 'deletePageModalCtrl',
            resolve: {
                page: function() {
                    return $scope.page;
                },
                pages: function() {
                    return $scope.pages;
                }
            }
        });

    };


})