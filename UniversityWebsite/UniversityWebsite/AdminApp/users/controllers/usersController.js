angular.module('configApp.users')

.controller('usersCtrl', function ($scope, $state, $location, userService) {
    $scope.totalUsers = 0;
    $scope.currentPage = 1;
    $scope.usersPerPage = 8;

    $scope.addUser = function () {
        $state.go("users.edit", { userId: "newUser", user: { Id: null } });
    };

    getPage(1);

    $scope.pageChanged = function (newPage) {
        getPage(newPage);
    };

    $scope.pagination = {
        current: 1
    };

    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.usersPerPage;
        userService.queryUsers(null, $scope.usersPerPage, offset).then(function (response) {
            $scope.totalUsers = response.data.Number;
            $scope.users = response.data.Elements;
        });
    };
})