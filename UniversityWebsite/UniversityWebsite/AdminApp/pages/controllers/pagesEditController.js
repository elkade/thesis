angular.module('configApp.pages')


.controller('pagesEditCtrl', function ($scope, $stateParams, $modal, $location, utils, pagesService) {
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

    $scope.update = function (event) {
        $scope.errors = [];

        if ($scope.pageForm.$valid) {
            preUpdatePage($scope.page);

            if ($scope.page != null && $scope.page.Id != null) {
                pagesService.update({ id: $scope.page.Id }, $scope.page, function(response) {
                    $scope.state = response.$resolved ? 'success' : 'error';
                    $scope.page = response;
                }, errorHandler);
            } else {
                var page = $scope.page || new Object();
                pagesService.post(page, function(response) {
                    $scope.state = response.$resolved ? 'success' : 'error';
                    $scope.page = response;
                    $scope.pages.push($scope.page);
                }, errorHandler);
            }
        } else {
            //utils.showValidation($scope.pageForm);
        }
    };

    $scope.tinymceOptions = {
        height: 500,
        plugins: 'textcolor link code image',
        toolbar: "undo redo styleselect bold italic forecolor backcolor code image",
        menu: { // this is the complete default configuration
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link media | template hr' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        },
        file_browser_callback: myFileBrowser
    };

    function myFileBrowser(field_name, url, type, win) {
        console.log("dasd");
        
        // alert("Field_Name: " + field_name + "nURL: " + url + "nType: " + type + "nWin: " + win); // debug/testing

        /* If you work with sessions in PHP and your client doesn't accept cookies you might need to carry
           the session name and session ID in the request string (can look like this: "?PHPSESSID=88p0n70s9dsknra96qhuk6etm5").
           These lines of code extract the necessary parameters and add them back to the filebrowser URL again. */

        var cmsURL = window.location.pathname;      // script URL
        var searchString = window.location.search;  // possible parameters
        if (searchString.length < 1) {
            // add "?" to the URL to include parameters (in other words: create a search string because there wasn't one before)
            searchString = "?";
        }

        if (cmsURL.indexOf("?") < 0) {
            //add the type as the only query parameter
            cmsURL = cmsURL + "?type=" + type;
        }
        else {
            //add the type as an additional query parameter
            // (PHP session ID is now included if there is one at all)
            cmsURL = cmsURL + "&type=" + type;
        }

        tinyMCE.activeEditor.windowManager.open({
            file: cmsURL + searchString + "&type=" + type,
            title: "File Browser",
            width: 420,
            height: 400,
            close_previous: "no"
        }, {
            window: win,
            input: field_name,
            resizable: "yes",
            inline: "yes",
        });
        return false;
    }

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