angular.module('configApp.menus', ['ui.router', 'configApp.menus.service', 'configApp.pages.service'])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            var getPages = function (pagesService) {
                return pagesService.all();
            };
            $stateProvider
                .state('menus', {
                    url: '/menus',
                    templateUrl: 'adminapp/views/menus/menus.html',

                    resolve: {
                        menus: [
                            'menus',
                            function (menus) {
                                return menus.all();
                            }
                        ],
                        pages: getPages
                    },

                    controller: 'menusEditCtrl'

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