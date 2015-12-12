angular.module('configApp.languages')

.controller('langWizardCtrl', function ($scope, $state, translationKeys, Languages, $location, utils) {
    $scope.language = { Words: prepareTranslationList(translationKeys), CountryCode: null };

    $scope.steps = new Object();
    $scope.steps["languageForm.basic"] =
    {
        code: "languageForm.basic",
        name: "Basic info",
        sref: ".basic",
        progress: "50%",
        isActive: function() { return $state.current.name == "languageForm.basic"; }
    };
    $scope.steps["languageForm.translations"] =
    {
        code: "languageForm.translations",
        name: "Translations",
        sref: ".translations",
        progress: "100%",
        isActive: function() { return $state.current.name == "languageForm.translations"; }
    };

    $scope.firstStep = Object.keys($scope.steps)[0];
    $scope.lastStep = Object.keys($scope.steps)[Object.keys($scope.steps).length - 1];

    $scope.isBackDisabled = function() {
        return $scope.currentStep() == $scope.steps[0];
    };

    $scope.isNextDisabled = function () {
        return $scope.currentStep().name == $scope.steps[steps.length - 1].name;
    };

    $scope.isFinishDisabled = function () {
        return $scope.currentStep() != $scope.steps[steps.length - 1];
    };

    $scope.back = function () {
        $location.path('languageForm/basic');
    };

    $scope.next = function() {
        $location.path('languageForm/translations');
    };

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