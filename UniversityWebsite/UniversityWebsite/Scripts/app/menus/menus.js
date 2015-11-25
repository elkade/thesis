angular.module('configApp.menus', ['ui.router', 'configApp.menus.service'])
    .factory('Menus', ['$resource', function ($resource) {
        return $resource('api/menu/:id', {}, {
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
                        ]
                    },

                    controller: [
                        '$scope', '$state', 'menus',
                        function($scope, $state, menus) {
                            $scope.menus = menus;
                        }
                    ]
                        
                })
                .state('menus.edit', {
                    url: '/:id',

                    views: {
                        '': {
                            templateUrl: 'scripts/app/views/menus/menus.edit.html',
                            controller: [
                                '$scope', '$stateParams', 'utils', 'Menus',
                                function ($scope, $stateParams, utils, Menus) {
                                    $scope.menu = utils.findByName($scope.menus, $stateParams.id);
                                }
                            ]
                        },
                    }
                });
        }
    ]);