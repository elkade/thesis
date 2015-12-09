angular.module('configApp.languages')

.controller('newLangModalCtrl', function ($scope, $modalInstance, translationKeys, Languages, $location) {
    $scope.language = {Words: prepareTranslationList(translationKeys), CountryCode: null};

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.ok = function () {
        console.log($scope.language);
        Languages.post({ lang: $scope.language.CountryCode }, $scope.language, function (response) {
            $modalInstance.close(response);
        }, errorHandler);
        
    };

    function prepareTranslationList(translationKeys) {
        var translations = new Object();
        for (var i = 0; i < translationKeys.length; i++) {
            translations[translationKeys[i]] = "";
        }
        return translations;
    }

    var errorHandler = function (response) {
    };

})