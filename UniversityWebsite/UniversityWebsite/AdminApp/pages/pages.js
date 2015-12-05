angular.module('configApp.pages', ['ui.router', 'configApp.pages.service'])
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function($stateProvider, $urlRouterProvider) {
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
                        ]
                    },

                    controller: [
                        '$scope', '$state', 'pages', '$location',
                        function($scope, $state, pages, $location) {
                            $scope.pages = pages;

                            $scope.add = function() {
                                $scope.page = new Object();
                                $scope.page.Id = null;
                                console.log($scope.page);
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