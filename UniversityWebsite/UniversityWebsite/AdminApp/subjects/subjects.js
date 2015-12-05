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
                        templateUrl: 'adminapp/views/subjects/subjects.edit.html',
                        controller: [
                            '$scope', '$stateParams', 'utils', 'subjectsPost',
                            function ($scope, $stateParams, utils, subjectsPost) {
                                $scope.subject = utils.findByName($scope.subjects, $stateParams.subjectName);

                                $scope.oneAtATime = true;

                                $scope.tinymceOptions = {
                                    lplugins: 'textcolor link code',
                                    toolbar: "undo redo styleselect bold italic forecolor backcolor code",
                                    inline: true,
                                    menu: { // this is the complete default configuration
                                        edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
                                        insert: { title: 'Insert', items: 'link media | template hr' },
                                        format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
                                    }
                                };

                                $scope.subject.NewsContent = "Panel 1. Lorem ipsum dolor sit amet, consectetur adipisicing eaccordion-groupt, sed do eiusmod tempor incididunt ut labore et dolore magna aaccordion-groupqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aaccordion-groupquip ex ea commodo consequat.";
                            }


                        ]
                    },
                }
            });
        }
    ]);