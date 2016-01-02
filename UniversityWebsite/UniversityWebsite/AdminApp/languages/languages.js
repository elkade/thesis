angular.module('configApp.languages', ['ui.router', 'configApp.languages.service'])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            var getLanguages = function(languageService) {
                return languageService.allLanguages();
            };

            var getDictionaries = function (languageService) {
                return languageService.allDictionaries();
            };

            var translationKeys = function (languageService) {
                return languageService.allTranslationKeys();
            };

            $stateProvider
                .state('languages', {
                    url: '/languages',
                    templateUrl: 'adminapp/views/languages/languages.html',

                    resolve: {
                        languages: getLanguages,
                        dictionaries: getDictionaries
                    },
                    data: { auth: "admin"},
                    controller: 'languagesCtrl'

                })
                .state('languageForm', {
                    url: '/languageForm',
                    templateUrl: 'adminapp/views/languages/languageWizard.html',
                    data: { auth: "admin"},
                    resolve: {
                        translationKeys: translationKeys
                    },

                    controller: 'langWizardCtrl'
                })
                .state('languageForm.basic', {
                    url: '/basic',
                    templateUrl: 'adminapp/views/languages/languageWizard.basic.html',
                    data: { auth: "admin" }
                })
                .state('languageForm.translations', {
                    url: '/translations',
                    templateUrl: 'adminapp/views/languages/languageWizard.translations.html',
                    data: { auth: "admin" }
                });
            //$urlRouterProvider.otherwise('/languageForm/basic');
        }
    ]);