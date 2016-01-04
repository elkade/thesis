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

    $scope.signOut = function() {
        var selectedStudents = Enumerable.From($scope.students).Where(function(student) {
            return student.selected;
        }).Select(function(student) {
            return student.Id;
        }).ToArray();

        if (selectedStudents.length > 0) {
            subjectsService.signOutStudents($scope.subject.Id, selectedStudents).then(function () {
                var alert = { type: 'success', msg: 'Selected students were successfully removed from the subject.' };
                $scope.addAlert(alert);
                getPage(1);
            }, function (error) {
                var alert = { type: 'alert', msg: 'Error: ' + error };
                $scope.addAlert(alert);
            });
        }
    };
 
    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.studentsPerPage;
        subjectsService.queryStudents($scope.subject.Id, $scope.studentsPerPage, offset).$promise.then(function (resp){
            $scope.students = resp.Elements;
            $scope.totalStudents = resp.Number;
        });
    };
});