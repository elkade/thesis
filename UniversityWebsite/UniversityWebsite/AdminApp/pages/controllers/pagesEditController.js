angular.module('configApp.pages')

.controller('pagesEditCtrl', function ($scope, $stateParams, $modal, $location, utils, pagesService, page, $state) {
    $scope.page = page;
    $scope.availableLanguages = findAvailableLanguages($scope.languages, $scope.page.Translations);
    $scope.alerts = [];

    var preUpdatePage = function(page) {
        if (!page.UrlName) {
            page.UrlName = null;
        }
    };

    /**
     * Sends current page data to server.
     */
    $scope.update = function () {
        $scope.errors = [];

        if ($scope.pageForm.$valid) {
            preUpdatePage($scope.page);

            if ($scope.page != null && $scope.page.Id != null) {
                pagesService.update({ id: $scope.page.Id }, $scope.page, function (response) {
                    addSuccesAlert();
                    $scope.page = response;
                }, errorHandler);
            } else {
                var page = $scope.page || new Object();
                pagesService.post(page, function (response) {
                    addSuccesAlert();
                    $scope.page = response;
                }, errorHandler);
            }
        } else {
            utils.showValidation($scope.pageForm);
        }
    };

    $scope.tinymceOptions = {
        height: 500,
        plugins: 'textcolor code advlist autolink lists link image charmap print preview anchor',
        toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | forecolor backcolor | code",
        menu: {
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link media | template hr' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        },
        file_browser_callback: function (field_name, url, type, win) {
            tinymce.activeEditor.windowManager.open({
                title: "My file browser",
                url: "/adminapp/views/galleryPopup.html",
                width: 800,
                height: 600
            }, {
                oninsert: function (url) {
                    win.document.getElementById(field_name).value = url;
                }
            });
        }
    };

    /**
     * Opens confirmation window before delete current page.
     */
    $scope.openDeleteModal = function () {
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
        if (existingTranslations == null || existingTranslations.Empty) {
            return languages;
        }

        return Enumerable.From(languages)
            .Where(function(lan) {
                return Enumerable.From(existingTranslations).All(function(t) {
                    return t.CountryCode != lan.CountryCode;
                });
            })
            .ToArray();
    };

    $scope.newTranslationAvailable = function() {
        return $scope.page.Translations != null && $scope.page.Translations.length < $scope.languages.length - 1;
    };

    /**
     * Prepares translated page and redirects to edit of translation.
     */
    $scope.addTranslation = function() {
        var translations = $scope.page.Translations;
        translations.push({
            Id: $scope.page.Id,
            Title: $scope.page.Title,
            UrlName: $scope.page.UrlName,
            CountryCode: $scope.page.CountryCode
        });

        var newTranslation =  {
            Id: null,
            GroupId: $scope.page.GroupId,
            Translations: translations
        };
        $state.go("pages.edit", { pageName: "translate" + $scope.page.UrlName, page: newTranslation });
        $scope.closeAlert(1);
    };

    /**
     * Removes alert with given idex
     * @param {int} index
     */
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    /**
     * Adds alert to set of allerts assosiated with edited page.
     * @param {type} alert
     */
    $scope.addAlert = function (alert) {
        $scope.alerts.push(alert);
    };

    var errorHandler = function (response) {
        var errors = utils.parseErrors(response.data.ModelState);
        for (var i = 0; i < errors.length; i++) {
            var alert = { type: 'alert', msg: 'Error: ' + errors[i]};
            $scope.addAlert(alert);
        }
    };

    var addSuccesAlert = function() {
        var alert = { type: 'success', msg: 'The ' + $scope.page.Title + ' page has been updated.' };
        $scope.addAlert(alert);
    };
})