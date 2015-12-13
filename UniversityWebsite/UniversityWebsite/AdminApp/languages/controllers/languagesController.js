angular.module('configApp.menus')

.controller('languagesCtrl', function ($scope, $state, languages, dictionaries, $modal, languageService, $location) {
    $scope.languages = languages;
    $scope.dictionaries = dictionaries;
    $scope.language = {};

    $scope.translations = extractTranslations(dictionaries);

    $scope.translationList = { name: 'translations', url: 'adminapp/views/languages/translations.html' };

    $scope.edit = function(language) {
        $scope.language = language;
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

    function extractDictionaries(translations) {
        var result = new Object();

        Enumerable.From($scope.translations).ForEach(function(translation) {
            for (var i = 0; i < translation.Value.length; i++) {
                var word = translation.Value[i];
                if (result[word.Lang] == null) {
                    result[word.Lang] = new Object();
                }

                var dic = result[word.Lang];
                dic[translation.Key] = word.Name;
            }            
        });

        var dictionaries = [];
        Enumerable.From(result).ForEach(function(res) {
            var dictionary = new Object();
            dictionary.CountryCode = res.Key;
            dictionary.Words = res.Value;
            dictionaries.push(dictionary);
        });

        console.log(dictionaries);
        return dictionaries;
    };

    $scope.updateTranslations = function() {
        var dictionaries = extractDictionaries($scope.translations);

        languageService.updateDictionaries(dictionaries, function (response) {
            //TODO: updated
        }, errorHandler);
    };

    var errorHandler = function (response) {
    };

    //$scope.open = function () {
    //    var modalInstance = $modal.open({
    //        templateUrl: 'adminapp/views/languages/newLangModal.html',
    //        controller: 'newLangModalCtrl',
    //        resolve: {
    //            translationKeys: [
    //                'dictionaries',
    //                function (dictionaries) {
    //                    return dictionaries.allTranslationKeys();
    //                }
    //            ],
    //        }
    //    });

    //    modalInstance.result.then(function(result) {
    //        if (result != null) {
    //            languageService.refresh();
    //            $scope.languages = languageService.allLanguages();
    //            $state.reload();
    //        }
    //    }, function () { //if modal dismis
            
    //    });
    //};

    $scope.startWizard = function() {
        $location.path('languageForm/basic');
    };

})