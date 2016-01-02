angular.module('configApp.subjects', ['ui.router', 'configApp.subjects.service'])
    .factory('subjectsPost', function ($resource) {
        return $resource('/api/subjects/:id');
    })
    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            $stateProvider
                .state('subjects', {
                    url: '/subjects',
                    templateUrl: 'adminapp/views/subjects/subjects.html',
                    controller: 'subjectsCtrl',
                    data: { auth: "" },
            })
            .state('subjects.edit', {
                url: '/:subjectName',
                params: {
                    subject : null
                },
                data: { auth: "" },
                views: {
                    '': {
                        templateUrl: 'adminapp/views/subjects/subjects.edit.html',
                        controller: 'subjectsEditCtrl'
                    },
                }
            });
        }
    ]);