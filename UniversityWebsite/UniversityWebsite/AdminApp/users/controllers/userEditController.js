angular.module('configApp.pages')

.controller('userEditCtrl', function ($scope, $stateParams, $state, utils,  userService) {
    $scope.alerts = [];
    $scope.roles = ["Administrator", "Student", "Teacher"];
    $scope.user = $stateParams.user;
    if ($scope.user.Password != null) {
        var alert = { type: 'success', msg: 'User ' + $scope.user.FirstName + " " + $scope.user.LastName + ' has been added. \n GENERATED PASSWORD: ' + $scope.user.Password };
        addAlert(alert);
    }
    $scope.changePassword = ($stateParams.userId == "newUser");

    $scope.update = function () {
        $scope.alerts = [];
        if ($scope.userForm.$valid) {
            if ($stateParams.userId == "newUser") {
                $scope.user = $scope.user || new Object();
                userService.post($scope.user, function (response) {
                    $scope.user = response;
                    $scope.users.push($scope.user);                    
                    $state.go("users.edit", { userId: $scope.user.Id, user: $scope.user});
                }, errorHandler);
            } else {
                userService.update({ id: $scope.user.Id }, $scope.user, function (response) {
                    $scope.user = response;
                    var alert = { type: 'success', msg: 'User ' + $scope.user.FirstName + " " + $scope.user.LastName + ' has been updated.' };
                    addAlert(alert);
                }, errorHandler);
            }
        } else {
            utils.showValidation($scope.userForm);
        }
    };

    $scope.disable = function () {
        userService.disableUser($scope.user.Id).then(function (resp) {
            var alert = { type: 'success', msg: 'User ' + $scope.user.FirstName + " " + $scope.user.LastName + ' has been disabled.' };
            addAlert(alert);
            $scope.user = resp.data;
        }, errorHandler);
    };

    $scope.activ = function () {
        console.log("disable");
        userService.activateUser($scope.user.Id).then(function (resp) {
            $scope.user = resp.data;
            var alert = { type: 'success', msg: 'User ' + $scope.user.FirstName + " " + $scope.user.LastName + ' has been activated. \n GENERATED PASSWORD: ' + $scope.user.Password };
            addAlert(alert);
        }, errorHandler);
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    function addAlert(alert) {
        $scope.alerts.push(alert);
    };

    var errorHandler = function(response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
        Enumerable.From($scope.errors).ForEach(function(error) {
            var alert = { type: 'alert', msg: error };
            addAlert(alert);
        });
    };
})