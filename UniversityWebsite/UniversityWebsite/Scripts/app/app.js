angular.module('configApp', [
    'ui.tinymce',
    'mm.foundation',
    'mm.foundation.accordion',
    'mm.foundation.modal',
    'configApp.pages',
    'configApp.subjects',
    'configApp.menus',
    'configApp.users',
    'configApp.utils.service',
    'ui.router',
    'ngResource',
    'ngAnimate'])

.directive('ncgRequestVerificationToken', ['$http', function ($http) {
    return function (scope, element, attrs) {
        $http.defaults.headers.common['RequestVerificationToken'] = attrs.ncgRequestVerificationToken || "no request verification token";
    };
}])

.run( 
    [            '$http', '$rootScope', '$state', '$stateParams', 
        function ($http, $rootScope,   $state,   $stateParams) {

        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
}])

.config(
[            '$stateProvider', '$urlRouterProvider',
    function ($stateProvider,   $urlRouterProvider) {

      /////////////////////////////
      // Redirects and Otherwise //
      /////////////////////////////

      // Use $urlRouterProvider to configure any redirects (when) and invalid urls (otherwise).
      $urlRouterProvider

        // The `when` method says if the url is ever the 1st param, then redirect to the 2nd param
        // Here we are just setting up some convenience urls.
        .when('/p?id', '/pages/:pageName')

        // If the url is ever invalid, e.g. '/asdf', then redirect to '/' aka the home state
        .otherwise('/');


      //////////////////////////
      // State Configurations //
      //////////////////////////

      // Use $stateProvider to configure your states.
    $stateProvider
        .state("dashboard", {
            url: "/",
            templateUrl: 'scripts/app/views/dashboard.html'
        })
}])


