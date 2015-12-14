
angular.module('configApp.pages')

.controller('userSubjectsEditCtrl', function ($scope, $stateParams, utils, userService) {

    $scope.user = utils.findById($scope.users, $stateParams.userId);

    $scope.update = function () {
        $scope.errors = [];
    };

    $scope.removeOwnedSubject = function(subject) {
        utils.remove($scope.user.OwnedSubjects, subject);
    };

    $scope.removeParticipatedSubject = function (subject) {
        utils.remove($scope.user.ParticipatedSubjects, subject);
    };

    var errorHandler = function () {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };
})