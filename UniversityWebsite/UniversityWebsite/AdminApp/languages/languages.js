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

            $stateProvider
                .state('languages', {
                    url: '/languages',
                    templateUrl: 'adminapp/views/languages/languages.html',

                    resolve: {
                        languages: getLanguages,
                        dictionaries: getDictionaries
                    },

                    controller: 'languagesCtrl'

                })
        }
    ]);