angular.module('configApp.menus', ['ui.router', 'configApp.menus.service'])
    .factory('menusApi', function ($resource) {
        return $resource('/api/menus/:id');
    })
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
                                '$scope', '$stateParams', 'utils', 'menusApi',
                                function ($scope, $stateParams, utils, menusApi) {
                                    $scope.update = function() {
                                        var pos = new post($scope.menu);
                                        pos.$save(function(response) {
                                            console.log(response.$resolved);
                                            
                                            $scope.state = response.$resolved ? 'success' : 'error';
                                        });
                                    }

                                    $scope.menu = utils.findByName($scope.menus, $stateParams.id);
                                }
                            ]
                        },
                    }
                });
        }
    ]);