angular.module('configApp.pages')

.controller('pagesEditCtrl', function ($scope, $stateParams, utils, Pages, $modal, $location) {
    if ($stateParams.pageName == "newPage") {
        $scope.page = { Id: null };
        
    } else if ($stateParams.pageName == 'newPageTranslation') {
        $scope.page = { Id: null, GroupId: window.GroupId };
    } else {
        $scope.page = utils.findByTitle($scope.pages, $stateParams.pageName);
    }

    var translations = Enumerable.From($scope.pages)
        .Where(function (p) {
            return p.GroupId == $scope.page.GroupId
                && p.CountryCode != $scope.page.CountryCode;
        });
    $scope.translations = translations.ToArray();

    $scope.availableLanguages = findAvailableLanguages($scope.languages, translations);

    var errorHandler = function(response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };

    var preUpdatePage = function(page) {
        if (!page.UrlName) {
            page.UrlName = null;
        }
    };

    $scope.update = function() {
        $scope.errors = [];

        if ($scope.pageForm.$valid) {
            preUpdatePage($scope.page);

            if ($scope.page != null && $scope.page.Id != null) {
                Pages.update({ id: $scope.page.Id }, $scope.page, function(response) {
                    $scope.state = response.$resolved ? 'success' : 'error';
                    $scope.page = response;
                }, errorHandler);
            } else {
                var page = $scope.page || new Object();
                Pages.post(page, function(response) {
                    $scope.state = response.$resolved ? 'success' : 'error';
                    $scope.page = response;
                    $scope.pages.push($scope.page);
                }, errorHandler);
            }
        } else {
            utils.showValidation($scope.pageForm);
        }
    };

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
            templateUrl: 'adminapp/views/pages/deletePageModal.html',
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

    function findAvailableLanguages(languages, existingTranslations) {
        if (existingTranslations.Empty) {
            return languages;
        }

        return Enumerable.From(languages)
            .Where(function(lan) {
                return translations.All(function(t) {
                    return t.CountryCode != lan.CountryCode;
                });
            })
            .ToArray();
    };

    $scope.add = function (groupId) {
        window.GroupId = groupId;
        $location.path('pages/newPageTranslation');
    };

    $scope.newTranslationAvailable = function() {
        return $scope.availableLanguages.length > 1;
    };

})