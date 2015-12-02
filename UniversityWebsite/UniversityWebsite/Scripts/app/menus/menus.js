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

                            $scope.sortItems = function(items) {
                                return items.sort(function (first, second) {
                                    return first.Order - second.Order;
                                });
                            },

                            $scope.addToMenu = function (page) {
                                var menuItem = new Object();
                                menuItem.Order = $scope.activeMenu.Items.length;
                                menuItem.Title = page.Title;
                                menuItem.PageId = page.Id;

                                console.log(menuItem.Order);
                                console.log($scope.activeMenu.CountryCode);
                                $scope.activeMenu.Items.push(menuItem);
                            },

                            $scope.moveUp = function (menuItem, items) {
                                items = $scope.sortItems(items);
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

                            $scope.remove = function (menuItem, items) {
                                items = $scope.sortItems(items);
                                utils.remove(items, menuItem);
                                for (var i = 0; i < items.length; i++) {
                                    items[i].Order = i;
                                }
                            },

                            $scope.update = function (menu) {
                                Menus.update({ lang: menu.CountryCode }, menu, function(response) {
                                    console.log("Udalo sie");
                                });
                            },

                            $scope.menuChanged = function(menu) {
                                $scope.activeMenu = menu;
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