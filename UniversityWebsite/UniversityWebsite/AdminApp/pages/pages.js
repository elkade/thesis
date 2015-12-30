angular.module('configApp.pages', ['ui.router', 'configApp.pages.service'])
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            var getLanguages = function (languageService) {
                return languageService.allLanguages();
            };

            $stateProvider
                .state('pages', {
                    url: '/pages',
                    templateUrl: 'adminapp/views/pages/pages.html',

                    resolve: {
                        languages: getLanguages
                    },

                    controller: [
                        '$scope', '$state', '$location', 'languages',
                        function ($scope, $state, $location, languages) {
                            $scope.languages = languages;

                            $scope.add = function() {
                                $state.go("pages.edit", {pageName: "newPage", page: {Id: null}} );
                            };
                        }
                    ]

                })
                .state('pages.edit', {
                    url: '/:pageName',
                    params: {
                        page: null
                    },
                    views: {
                        '': {
                            templateUrl: 'adminapp/views/pages/pages.edit.html',
                            controller: 'pagesEditCtrl'
                        },
                    }
                });

        }
    ]);