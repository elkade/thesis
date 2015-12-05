angular.module('configApp.main-pages', ['ui.router', 'configApp.main-pages.service', 'configApp.pages.service'])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('main-pages', {
                    url: '/main-pages',
                    templateUrl: 'adminapp/views/main-pages/main-pages.html',

                    resolve: {
                        menus: [
                            'mainPages',
                            function (mainPages) {
                                return mainPages.all();
                            }
                        ],
                        pages: [
                            'pages',
                            function (pages) {
                                return pages.all();
                            }
                        ]
                    },

                    controller: 'mainPagesEditCtrl'
                })
        }
    ]);