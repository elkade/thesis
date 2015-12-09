angular.module('configApp.menus')

.controller('languagesCtrl', function ($scope, $state, languages, dictionaries, $modal, Languages, languageService) {
    $scope.languages = languages;
    $scope.dictionaries = dictionaries;
    $scope.language = {};

    
    $scope.translations = extractTranslations(dictionaries);

    $scope.translationList = { name: 'translations', url: 'adminapp/views/languages/translations.html' };

    $scope.edit = function(language) {
        $scope.language = language;
    };

    var errorHandler = function (response) {
    };

    function extractTranslations(dictionaries) {
        var result = new Object();
        for (var i = 0; i < dictionaries.length; i++) {
            var dic = dictionaries[i];
            Enumerable.From(dic.Words).ForEach(function (word) {
                if (result[word.Key] == null) {
                    result[word.Key] = [];
                }
                var wordModel = { Name: word.Value, Lang: dic.CountryCode };
                result[word.Key].push(wordModel);
            });
        }
        return result;
    }

    $scope.save = function() {

    };

    $scope.open = function () {
        var modalInstance = $modal.open({
            templateUrl: 'adminapp/views/languages/newLangModal.html',
            controller: 'newLangModalCtrl',
            resolve: {
                translationKeys: [
                    'dictionaries',
                    function (dictionaries) {
                        return dictionaries.allTranslationKeys();
                    }
                ],
            }
        });

        modalInstance.result.then(function(result) {
            if (result != null) {
                languageService.refresh();
                $scope.languages = languageService.allLanguages();
                $state.reload();
            }
        }, function () { //if modal dismis
            
        });

    };

})