angular.module('configApp.menus', ['ui.router', 'configApp.menus.service', 'configApp.pages.service'])
    .factory('Menus', ['$resource', function ($resource) {
        return $resource('api/menu/main/:lang', {}, {
            query: { method: 'GET' },
            post: { method: 'POST' },
            update: { method: 'PUT' },
            remove: { method: 'DELETE' }
        });
    }])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('menus', {
                    url: '/menus',
                    templateUrl: 'scripts/app/views/menus/menus.html',

                    resolve: {
                        menus: [
                            'menus',
                            function (menus) {
                                return menus.all();
                            }
                        ],
                        pages: [
                            'pages',
                            function (pages) {
                                return pages.all();
                            }
                        ]
                    },

                    controller: [
                        '$scope', '$state', 'menus', 'pages',
                        function($scope, $state, menus, pages) {
                            $scope.menus = menus;
                            $scope.pages = pages;

                            $scope.activeMenu = menus[0];

                            $scope.addToMenu = function (page) {
                                console.log($scope.activeMenu)
                                $scope.activeMenu.Items.push(page);
                                
                            }
                        }
                    ]
                        
                })
                .state('menus.edit', {
                    url: '/:lang',

                    views: {
                        '': {
                            templateUrl: 'scripts/app/views/menus/menus.edit.html',
                            controller: [
                                '$scope', '$stateParams', 'utils', 'Menus',
                                function ($scope, $stateParams, utils, Menus) {
                                    $scope.menu = utils.findByCountryCode($scope.menus, $stateParams.lang);
                                }
                            ]
                        },
                    }
                });
        }
    ]);