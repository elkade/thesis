angular.module('configApp.subjects')

.controller('subjectTeachersCtrl', function ($scope, subjectsService, $modal, utils) {
    loadTeachers();

    $scope.update = function (teacher) {
        var teacherIds = [];
        teacherIds.push(teacher.Id);
        subjectsService.updateTeachers($scope.subject.Id, teacherIds).$promise.then(function (resp) {
            var alert = { type: 'success', msg: 'Teacher ' + teacher.FirstName + ' ' + teacher.LastName + ' was successfully added to the subject.' };
            $scope.addAlert(alert);
            loadTeachers();
        }, function (error) {
            errorHandler(error);
        });
    };

    $scope.remove = function() {
        var selectedTeachers = Enumerable.From($scope.teachers).Where(function(teacher) {
            return teacher.selected;
        }).Select(function(teacher) {
            return teacher.Id;
        }).ToArray();

        if (selectedTeachers.length > 0) {
            subjectsService.removeTeachers($scope.subject.Id, selectedTeachers).$promise.then(function (resp) {
                loadTeachers();
                var alert = { type: 'success', msg: 'Selected teachers were successfully removed from the subject.' };
                $scope.addAlert(alert);
            }, function (error) {
                errorHandler(error);
            });
        }
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

        modalInstance.result.then(function (selectedUser) {
            $scope.update(selectedUser);
        }, function () {
            console.log('Modal dismissed at: ' + new Date());
        });
    };

    function loadTeachers() {
        $scope.teachers = subjectsService.queryTeachers($scope.subject.Id);
    };

    var errorHandler = function (response) {
        var errors = utils.parseErrors(response.data.ModelState);
        for (var i = 0; i < errors.length; i++) {
            var alert = { type: 'alert', msg: 'Error: ' + errors[i] };
            $scope.addAlert(alert);
        }
    };
});