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
                        '$scope', '$state', 'menus', 'pages', 'utils', 'Menus',
                        function($scope, $state, menus, pages, utils, Menus) {
                            $scope.menus = menus;
                            $scope.pages = pages;

                            $scope.activeMenu = menus[0];

                            $scope.addToMenu = function (page) {
                                var menuItem = new Object();
                                menuItem.Order = $scope.activeMenu.Items.length;
                                menuItem.Title = page.Title;

                                console.log(menuItem.Order);
                                $scope.activeMenu.Items.push(menuItem);
                            },

                            $scope.moveUp = function (menuItem, items) {
                                items = items.sort(function(first, second) {
                                    return first.Order - second.Order;
                                });
                                menuItem.Order--;
                                items[menuItem.Order].Order++;
                            },

                            $scope.moveDown = function (menuItem, items) {
                                items = items.sort(function (first, second) {
                                    return first.Order - second.Order;
                                });
                                menuItem.Order++;
                                items[menuItem.Order].Order--;
                            },

                            $scope.remove = function(menuItem, items) {
                                utils.remove(items, menuItem);
                            },

                            $scope.update = function (menu) {
                                Menus.update({ lang: menu.CountryCode}, menu, function(response) {
                                    console.log("Udalo sie");
                                })
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