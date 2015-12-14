angular.module('configApp.pages')

.controller('userEditCtrl', function ($scope, $stateParams, utils,  userService) {

    $scope.user = utils.findById($scope.users, $stateParams.userId);

    $scope.update = function () {
        $scope.errors = [];

        if ($scope.userForm.$valid) {

            if ($stateParams.userId == "newUser") {
                $scope.users.push($scope.user);
                //$scope.user = $scope.user || new Object();
                //userService.post($scope.user, function (response) {
                //    $scope.user = response;
                //    $scope.users.push($scope.user);
                //}, errorHandler);
            } else {
                userService.update({ id: $scope.user.Id }, $scope.user, function (response) {
                    $scope.user = response;
                }, errorHandler);
            }
        } else {
            utils.showValidation($scope.userForm);
        }
    };

    var errorHandler = function() {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };
})