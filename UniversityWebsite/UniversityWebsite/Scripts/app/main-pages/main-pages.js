angular.module('configApp.main-pages', ['ui.router', 'configApp.main-pages.service', 'configApp.pages.service'])
    .factory('MainPages', ['$resource', function ($resource) {
        return $resource('api/tile/:lang', {}, {
            query: { method: 'GET' },
            post: { method: 'POST' },
            update: { method: 'PUT' },
            remove: { method: 'DELETE' }
        });
    }])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('main-pages', {
                    url: '/main-pages',
                    templateUrl: 'scripts/app/views/main-pages/main-pages.html',

                    resolve: {
                        mainPages: [
                            'mainPages',
                            function (mainPages) {
                                return mainPages.all();
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
                        '$scope', '$state', 'mainPages', 'pages', 'utils', 'MainPages',
                        function ($scope, $state, mainPages, pages, utils, MainPages) {
                            $scope.mainPages = mainPages;
                            $scope.pages = pages;

                            console.log(mainPages);

                            $scope.activeMenu = mainPages[0];

                            $scope.sortItems = function (items) {
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
                                MainPages.update({ lang: menu.CountryCode }, menu, function (response) {
                                    console.log("Udalo sie");
                                });
                            },

                            $scope.menuChanged = function (menu) {
                                $scope.activeMenu = menu;
                            }
                        }
                    ]

                })
        }
    ]);