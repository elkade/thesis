angular.module('configApp.pages', ['ui.router', 'configApp.pages.service'])
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            var getLanguages = function (languageService) {
                return languageService.allLanguages();
            };
            var getPages = function (pagesService) {
                return pagesService.all();
            };

            $stateProvider
                .state('pages', {
                    url: '/pages',
                    templateUrl: 'adminapp/views/pages/pages.html',

                    resolve: {
                        pages: getPages,
                        languages: getLanguages
                    },

                    controller: [
                        '$scope', '$state', 'pages', '$location', 'languages',
                        function ($scope, $state, pages, $location, languages) {
                            $scope.pages = pages.Elements;
                            //TODO: paginacja
                            $scope.languages = languages;

                            $scope.add = function() {
                                $state.go("pages.edit", {pageName: "newPage"} );
                            };
                        }
                    ]

                })
                .state('pages.edit', {
                    url: '/:pageName',
                    views: {
                        '': {
                            templateUrl: 'adminapp/views/pages/pages.edit.html',
                            controller: 'pagesEditCtrl'
                        },
                    }
                });

        }
    ]);