angular.module('configApp.pages')

.controller('pagesEditCtrl', function ($scope, $stateParams, utils, Pages, $modal) {

    var errorHandler = function (response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    }

    var preUpdatePage = function(page) {
        if (!page.UrlName) {
            page.UrlName = null;
        }
    }

    var showValidation = function(form) {
        window.angular.forEach(form.$error, function (field) {
            window.angular.forEach(field, function (errorField) {
                errorField.$setTouched();
            });
        });
    }

    $scope.update = function () {
        $scope.errors = [];

        if ($scope.pageForm.$valid) {
            preUpdatePage($scope.page);

            if ($scope.page != null && $scope.page.Id != null) {
                console.log($scope.page.Parent);
                Pages.update({ id: $scope.page.Id }, $scope.page, function (response) {
                    console.log(response.Parent);
                    $scope.state = response.$resolved ? 'success' : 'error';
                    $scope.page = response;
                }, errorHandler);
            } else {
                var page = $scope.page || new Object();
                Pages.post(page, function (response) {
                    $scope.state = response.$resolved ? 'success' : 'error';
                    $scope.page = response;
                    $scope.pages.push($scope.page);
                }, errorHandler);
            }
        } else {
            showValidation($scope.pageForm);
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