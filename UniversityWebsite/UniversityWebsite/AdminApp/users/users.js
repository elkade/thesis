angular.module('configApp.users', ['ui.router', 'configApp.users.service'])
    .factory('usersPost', function ($resource) {
        return $resource('/api/users/:id');
    })
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            $stateProvider
                .state('users', {
                    url: '/users',
                    templateUrl: 'adminapp/views/users/users.html',
                    controller: 'usersCtrl',
                    data: { auth: "admin" }
                })
                .state('users.edit', {
                    url: '/:userId',
                    params: {
                        user: null
                    },
                    views: {
                        '': {
                            templateUrl: 'adminapp/views/users/user.edit.html',
                            data: { auth: "admin" }
                        },
                    }
                });
        }
    ]);