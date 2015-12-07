angular.module('configApp.pages', ['ui.router', 'configApp.pages.service'])
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('pages', {
                    url: '/pages',
                    templateUrl: 'adminapp/views/pages/pages.html',

                    resolve: {
                        pages: [
                            'pages',
                            function(pages) {
                                return pages.all();
                            }
                        ],
                        languages: [
                            'languages',
                            function (languages) {
                                return languages.all();
                            }
                        ]
                    },

                    controller: [
                        '$scope', '$state', 'pages', '$location', 'languages',
                        function ($scope, $state, pages, $location, languages) {
                            $scope.pages = pages;
                            $scope.languages = languages;

                            $scope.add = function() {
                                $location.path('pages/newPage');
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