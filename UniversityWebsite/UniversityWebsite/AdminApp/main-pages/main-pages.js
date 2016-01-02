angular.module('configApp.main-pages', ['ui.router', 'configApp.main-pages.service', 'configApp.pages.service'])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            var getMainPages = function(mainPages) {
                return mainPages.all();
            };

            $stateProvider
                .state('main-pages', {
                    url: '/main-pages',
                    templateUrl: 'adminapp/views/main-pages/main-pages.html',

                    resolve: {
                        menus: getMainPages,
                    },
                    data: { auth: "admin"},
                    controller: 'mainPagesEditCtrl'
                });
        }
    ]);