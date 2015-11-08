angular.module('configApp', [
    'ui.tinymce',
    'configApp.pages',
    'configApp.pages.service',
    'configApp.utils.service',
    'ui.router',
    'ngResource',
    'ngAnimate'])

.run( 
    [            '$rootScope', '$state', '$stateParams', 
        function ($rootScope,   $state,   $stateParams) {

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
            templateUrl: 'admin/dashboard'
        })
        .state('users', {
            url: '/users',
            templateUrl: 'scripts/app/views/users.html'
        })
}])


