var configApp = angular.module('configApp', ['ngRoute']);

configApp.controller('LandingPageController', LandingPageController);

configApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);

var configFunction = function ($routeProvider, $httpProvider) {
    $routeProvider.
        when('/dashboard', {
            templateUrl: 'admin/dashboard'
        }).
        when('/users', {
            templateUrl: 'admin/users'
        }).
        when('/pages', {
            templateUrl: 'admin/pages'
        }).
        when('/subjects', {
            templateUrl: 'admin/subjects'
        }).
        when('/navigationMenu', {
            templateUrl: 'admin/navigationMenu'
        }).
        otherwise({
            redirectTo: '/dashboard'
        });

    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}
configFunction.$inject = ['$routeProvider', '$httpProvider'];

configApp.config(configFunction);