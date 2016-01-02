angular.module('configApp.subjects')

.controller('subjectTeachersCtrl', function ($scope, subjectsService) {
    loadTeachers();
    $scope.newTeacher = { edit: false, id: null };

    $scope.update = function () {
        var teacherIds = Enumerable.From($scope.teachers).Select(function (teacher) {
            return teacher.Id;
        }).ToArray();
        console.log(teacherIds);
        if ($scope.newTeacher.id != null) {
            teacherIds.push($scope.newTeacher.id);
        }
        subjectsService.updateTeachers($scope.subject.Id, teacherIds).$promise.then(function() {
            console.log("ok");
            $scope.cancel();
            loadTeachers();
        });
    };

    $scope.remove = function() {
        $scope.update();
        loadTeachers();
    };

    $scope.add = function () {
        console.log("add");
        $scope.newTeacher.edit = true;
    };

    $scope.cancel = function () {
        $scope.newTeacher.edit = false;
        $scope.newTeacher.id = null;
    };

    function loadTeachers() {
        $scope.teachers = subjectsService.queryTeachers($scope.subject.Id);
    };

    var errorHandler = function (response) {
        
    };
});