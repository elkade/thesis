angular.module('configApp.subjects', ['ui.router', 'configApp.subjects.service'])
    .factory('post', function ($resource) {
        return $resource('/api/subjects/:id');
    })
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('subjects', {
                    url: '/subjects',
                    templateUrl: 'scripts/app/views/subjects/subjects.html',

                    resolve: {
                        subjects: [
                            'subjects',
                            function (subjects) {
                                return subjects.all();
                            }
                        ]
                    },

                    controller: [
                        '$scope', '$state', 'subjects',
                        function ($scope, $state, subjects) {
                            $scope.subjects = subjects;
                            console.log($scope.subjects);
                        }
                    ]
            })
            .state('subjects.edit', {
                url: '/:subjectName',

                views: {
                    '': {
                        templateUrl: 'scripts/app/views/subjects/subjects.edit.html',
                        controller: [
                            '$scope', '$stateParams', 'utils', 'post',
                            function ($scope, $stateParams, utils, post) {
                                $scope.subject = utils.findByName($scope.subjects, $stateParams.subjectName);

                                $scope.oneAtATime = true;

                                $scope.subject.NewsContent = "Panel 1. Lorem ipsum dolor sit amet, consectetur adipisicing eaccordion-groupt, sed do eiusmod tempor incididunt ut labore et dolore magna aaccordion-groupqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aaccordion-groupquip ex ea commodo consequat.";
                            }


                        ]
                    },
                }
            });
        }
    ]);