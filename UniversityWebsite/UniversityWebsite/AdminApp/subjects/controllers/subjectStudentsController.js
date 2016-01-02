angular.module('configApp.subjects')

.controller('subjectStudentsCtrl', function ($scope, subjectsService, utils, $timeout) {
    $scope.totalStudents = 10;
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
        console.log($scope.subject.Id);
        //subjectsService.getStudents({ subjectId: $scope.subject.Id, limit: $scope.studentsPerPage, offset: offset }, function (students) {
        //    $scope.students = students;
        //});
    };
});