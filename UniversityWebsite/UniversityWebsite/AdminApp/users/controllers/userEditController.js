angular.module('configApp.pages')

.controller('userEditCtrl', function ($scope, $stateParams, utils,  userService) {

    $scope.alerts = [];
    $scope.user = utils.findById($scope.users, $stateParams.userId);
    $scope.roles = ["Administrator", "Student", "Teacher"];
    $scope.changePassword = false;
    if ($stateParams.userId == "newUser") {
        $scope.changePassword = true;
    }

    $scope.update = function () {
        $scope.alerts = [];

        if ($scope.userForm.$valid) {

            if ($stateParams.userId == "newUser") {
                $scope.user = $scope.user || new Object();
                userService.post($scope.user, function (response) {
                    $scope.user = response;
                    $scope.users.push($scope.user);
                    var alert = { type: 'success', msg: 'User ' + $scope.user.FirstName + " " + $scope.user.LastName + ' menu has been added. \n GENERATED PASSWORD: ' + $scope.user.Password};
                    $scope.addAlert(alert);
                }, errorHandler);
            } else {
                userService.update({ id: $scope.user.Id }, $scope.user, function (response) {
                    $scope.user = response;
                    var alert = { type: 'success', msg: 'User ' + $scope.user.FirstName + " " + $scope.user.LastName + ' has been updated.' };
                    $scope.addAlert(alert);
                }, errorHandler);
            }
        } else {
            utils.showValidation($scope.userForm);
        }
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.addAlert = function (alert) {
        $scope.alerts.push(alert);
    };

    var errorHandler = function(response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
        Enumerable.From($scope.errors).ForEach(function(error) {
            var alert = { type: 'alert', msg: error };
            console.log(alert);
            $scope.addAlert(alert);
        });
    };
})