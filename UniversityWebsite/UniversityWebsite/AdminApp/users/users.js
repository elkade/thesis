angular.module('configApp.users', ['ui.router', 'configApp.users.service'])
    .factory('usersPost', function ($resource) {
        return $resource('/api/users/:id');
    })
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            var getUsers = function (userService) {
                return userService.allUsers();
            };

            $stateProvider
                .state('users', {
                    url: '/users',
                    templateUrl: 'adminapp/views/users/users.html',
                    resolve: {
                        users: getUsers
                    },
                    controller: [
                        '$scope', '$state', '$location', 'users', 
                        function ($scope, $state, $location, users) {
                            $scope.users = users;

                            $scope.addUser = function () {
                                $location.path('users/newUser');
                            };
                        }
                    ]

                })
                .state('users.edit', {
                    url: '/:userId',

                    views: {
                        '': {
                            templateUrl: 'adminapp/views/users/user.edit.html',
                            resolve: {
                                users: getUsers
                            },
                            //controller: 'userEditCtrl'
                        },
                    }
                });
        }
    ]);