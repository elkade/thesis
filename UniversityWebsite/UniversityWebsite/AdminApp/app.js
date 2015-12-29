var configApp = angular.module('configApp', [
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
    'ngResource'
]);

configApp.filter("sanitize", ['$sce', function ($sce) {
    return function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    };
}]);

configApp.run(['$http', '$rootScope', '$state', '$stateParams', 'usSpinnerService',
    function ($http, $rootScope, $state, $stateParams, usSpinnerService) {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        $rootScope.$on('$stateChangeStart',
            function (event, toState, toParams, fromState, fromParams) {
                usSpinnerService.spin('spinner-1');
            });
        $rootScope.$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                usSpinnerService.stop('spinner-1');
            });
    }
]);

configApp.config(
['$stateProvider', '$urlRouterProvider',
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

        $stateProvider
            .state("dashboard", {
                url: "/",
                templateUrl: 'adminapp/views/dashboard.html'
            });
    }]);


