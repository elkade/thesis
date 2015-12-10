angular.module('configApp.languages')

.controller('langWizardCtrl', function ($scope, translationKeys, Languages, $location, utils) {
    $scope.language = {Words: prepareTranslationList(translationKeys), CountryCode: null};

    //$scope.cancel = function () {
    //    $modalInstance.dismiss('cancel');
    //};

    $scope.finish = function () {
        $scope.errors = [];
        console.log($scope.language);
        Languages.post({ lang: $scope.language.CountryCode }, $scope.language, function (response) {
            $location.path('languages');
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
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };

})