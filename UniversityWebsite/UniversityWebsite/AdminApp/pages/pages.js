angular.module('configApp.pages', ['ui.router', 'configApp.pages.service'])
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            var getLanguages = function (languageService) {
                return languageService.allLanguages();
            };

            $stateProvider
                .state('pages', {
                    url: '/pages',
                    templateUrl: 'adminapp/views/pages/pages.html',
                    data: { auth: "admin"},
                    resolve: {
                        languages: getLanguages
                    },
                    controller: [
                        '$scope', '$state', '$location', 'languages',
                        function($scope, $state, $location, languages) {
                            $scope.languages = languages;

                            $scope.add = function() {
                                $state.go("pages.edit", { pageName: "newPage", page: { Id: null } });
                            };

                            $scope.pageChanged = function (pages) {
                                $scope.pages = pages;
                            };
                        }
                    ]
                })
                .state('pages.edit', {
                    url: '/:pageName',
                    params: {
                        page: null
                    },
                    views: {
                        '': {
                            templateUrl: 'adminapp/views/pages/pages.edit.html',
                            controller: 'pagesEditCtrl',
                            data: { auth: "admin"},
                            resolve: {
                                pagesService: "pagesService",
                                $stateParams: "$stateParams",
                                page: function (pagesService, $stateParams) {
                                    var page = $stateParams.page;
                                    if (page.Id != null) {
                                        return pagesService.findPage(page.Id).$promise.then(function (response) {
                                            return response;
                                        });
                                    }
                                    return page;
                                }
                            },
                        },
                    }
                });

        }
    ]);