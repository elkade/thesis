angular.module('configApp.subjects')

.controller('subjectStudentsCtrl', function ($scope, subjectsService, utils, $timeout) {
    $scope.totalStudents = 0;
    $scope.currentPage = 1;
    $scope.studentsPerPage = 8;

    getPage(1);

    $scope.pageChanged = function (newPage) {
        getPage(newPage);
    };

    $scope.pagination = {
        current: 1
    };

    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.studentsPerPage;
        subjectsService.queryStudents($scope.subject.Id, $scope.studentsPerPage, offset).$promise.then(function (resp){
            $scope.students = resp.Elements;
            $scope.totalStudents = resp.Number;
        });
    };
});