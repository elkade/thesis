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
                    templateUrl: 'adminapp/views/users/users.html'
            })
        }
    ]);