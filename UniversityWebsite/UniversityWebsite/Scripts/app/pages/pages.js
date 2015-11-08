angular.module('configApp.pages', ['ui.router'])
    .factory('post', function ($resource) {
        return $resource('/api/pages/:id');
    })
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('pages', {
                    url: '/pages',
                    templateUrl: 'scripts/app/views/pages.html',

                    resolve: {
                        pages: [
                            'pages',
                            function(pages) {
                                return pages.all();
                            }
                        ]
                    },

                    controller: [
                        '$scope', '$state', 'pages',
                        function($scope, $state, pages) {
                            $scope.pages = pages;
                        }
                    ]
                        
                })
                .state('pages.edit', {
                    url: '/:pageName',

                    views: {
                        '': {
                            templateUrl: 'scripts/app/views/pages.edit.html',
                            controller: [
                                '$scope', '$stateParams', 'utils', 'post',
                                function ($scope, $stateParams, utils, post) {
                                    $scope.update = function() {
                                        var pos = new post($scope.page);
                                        pos.$save(function(response) {
                                            console.log(response.$resolved);
                                            
                                            $scope.state = response.$resolved ? 'success' : 'error';
                                        });
                                    }

                                    $scope.page = utils.findByName($scope.pages, $stateParams.pageName);
                                }
                            ]
                        },
                    }
                });
        }
    ]);