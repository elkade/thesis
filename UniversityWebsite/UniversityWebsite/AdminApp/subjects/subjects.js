angular.module('configApp.subjects', ['ui.router', 'configApp.subjects.service'])
    .factory('subjectsPost', function ($resource) {
        return $resource('/api/subjects/:id');
    })
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            var getSubjects = function(subjectsService) {
                return subjectsService.all();
            };

            $stateProvider
                .state('subjects', {
                    url: '/subjects',
                    templateUrl: 'adminapp/views/subjects/subjects.html',

                    resolve: {
                        subjects: getSubjects
                    },

                    controller: [
                        '$scope', '$state', 'subjects', '$location',
                        function ($scope, $state, subjects, $location) {
                            $scope.subjects = subjects;
                            
                            $scope.add = function () {
                                $location.path('subjects/newSubject');
                            };

                        }
                    ]
            })
            .state('subjects.edit', {
                url: '/:subjectName',

                views: {
                    '': {
                        templateUrl: 'adminapp/views/subjects/subjects.edit.html',
                        controller: 'subjectsEditCtrl'
                    },
                }
            });
        }
    ]);