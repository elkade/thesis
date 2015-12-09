angular.module('configApp.languages', ['ui.router', 'configApp.languages.service'])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('languages', {
                    url: '/languages',
                    templateUrl: 'adminapp/views/languages/languages.html',

                    resolve: {
                        languages: [
                            'languages',
                            function (languages) {
                                return languages.all();
                            }
                        ],
                        dictionaries: [
                            'dictionaries',
                            function (dictionaries) {
                                return dictionaries.all();
                            }
                        ]
                    },

                    controller: 'languagesCtrl'

                })
        }
    ]);