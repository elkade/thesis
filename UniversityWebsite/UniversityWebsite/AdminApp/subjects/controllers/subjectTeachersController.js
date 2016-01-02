angular.module('configApp.subjects')

.controller('subjectTeachersCtrl', function ($scope, subjectsService, $modal) {
    loadTeachers();

    $scope.update = function (teacherId) {
        var teacherIds = [];
        teacherIds.push(teacherId);
        subjectsService.updateTeachers($scope.subject.Id, teacherIds).$promise.then(function(resp) {
            loadTeachers();
        });
    };

    $scope.remove = function() {
        var selectedTeachers = Enumerable.From($scope.teachers).Where(function(teacher) {
            return teacher.selected;
        }).Select(function(teacher) {
            return teacher.Id;
        }).ToArray();
        subjectsService.removeTeachers($scope.subject.Id, selectedTeachers).$promise.then(function(resp) {
            loadTeachers();
        }, function(error) {
            
        });
    };

    $scope.add = function () {
        $scope.openUsersModal();
    };

    $scope.openUsersModal = function () {
        var modalInstance = $modal.open({
            templateUrl: 'adminapp/views/users/users.modal.html',
            controller: 'usersModalCtrl',
            windowClass: 'small'
        });

        modalInstance.result.then(function (selectedUserId) {
            $scope.update(selectedUserId);
        }, function () {
            console.log('Modal dismissed at: ' + new Date());
        });
    };

    function loadTeachers() {
        $scope.teachers = subjectsService.queryTeachers($scope.subject.Id);
    };

    var errorHandler = function (response) {
        
    };
});