angular.module('configApp.menus')

.controller('languagesCtrl', function ($scope, $state, languages, dictionaries, utils, Languages) {
    $scope.languages = languages;
    $scope.dictionaries = dictionaries;
    $scope.language = {};

    
    $scope.translations = extractTranslations(dictionaries);

    console.log($scope.translations);

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
        //if ($scope.languageForm.$valid) {
        console.log($scope.language);
        console.log($scope.languages);
            Languages.post({ lang: $scope.language.CountryCode }, $scope.language, function (response) {
                console.log(response);
                $scope.language = null;
                $scope.languages.push(response);
            }, errorHandler);
            //Languages.post(page, function (response) {
            //    $scope.state = response.$resolved ? 'success' : 'error';
            //    $scope.page = response;
            //    $scope.pages.push($scope.page);
            //}, errorHandler);
        //}
    };

})