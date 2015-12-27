angular.module('configApp', [
    'ui.tinymce',
    'ngFileUpload',
    'mm.foundation',
    'mm.foundation.accordion',
    'mm.foundation.modal',
    'angularUtils.directives.dirPagination',
    'ngAnimate',
    'angularSpinner',
    'configApp.pages',
    'configApp.subjects',
    'configApp.menus',
    'configApp.main-pages',
    'configApp.users',
    'configApp.languages',
    'configApp.gallery',
    'configApp.utils.service',
    'configApp.files.service',
    'ui.router',
    'ngResource'])

.directive('ncgRequestVerificationToken', ['$http', function ($http) {
    return function (scope, element, attrs) {
        $http.defaults.headers.common['RequestVerificationToken'] = attrs.ncgRequestVerificationToken || "no request verification token";
    };
}])

.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function(scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            
            element.bind('change', function(){
                scope.$apply(function(){
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}])

.filter("sanitize", ['$sce', function($sce) {
    return function(htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    };
}])

.run([       '$http', '$rootScope', '$state', '$stateParams', 'usSpinnerService', 
    function ($http,   $rootScope,   $state,   $stateParams,   usSpinnerService) {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        $rootScope.$on('$stateChangeStart',
            function(event, toState, toParams, fromState, fromParams) {
                usSpinnerService.spin('spinner-1');
            });
        $rootScope.$on('$stateChangeSuccess',
            function(event, toState, toParams, fromState, fromParams) {
                usSpinnerService.stop('spinner-1');
            });
    }])

.config(
[            '$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {


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
            templateUrl: 'adminapp/views/dashboard.html'
        })
}])


