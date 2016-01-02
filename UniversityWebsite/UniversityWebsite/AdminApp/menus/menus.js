angular.module('configApp.menus', ['ui.router', 'configApp.menus.service', 'configApp.pages.service'])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            //var getPages = function (pagesService) {
            //    return pagesService.all();
            //};

            var getMenus = function(menuService) {
                return menuService.allMenus();
            };

            $stateProvider
                .state('menus', {
                    url: '/menus',
                    templateUrl: 'adminapp/views/menus/menus.html',
                    resolve: {
                        menus: getMenus,
                    },
                    controller: 'menusEditCtrl',
                    data: { auth: "admin"}

                })
                .state('menus.edit', {
                    url: '/:lang',
                    data: { auth: "admin"},
                    views: {
                        '': {
                            templateUrl: 'scripts/app/views/menus/menus.edit.html',
                            controller: [
                                '$scope', '$stateParams', 'utils',
                                function ($scope, $stateParams, utils) {
                                    $scope.menu = utils.findByCountryCode($scope.menus, $stateParams.lang);
                                }
                            ]
                        },
                    }
                });
        }
    ]);