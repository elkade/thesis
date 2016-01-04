angular.module('configApp.subjects')


.controller('subjectsCtrl', function ($scope, $state, $location, subjectsService, user) {
    $scope.user = user;
    $scope.totalSubjects = 0;
    $scope.currentPage = 1;
    $scope.subjectsPerPage = 8;

    getPage(1);

    $scope.add = function () {
        $location.path('subjects/newSubject');
    };

    $scope.pageChanged = function (newPage) {
        getPage(newPage);
    };

    $scope.pagination = {
        current: 1
    };

    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.subjectsPerPage;
        subjectsService.querySubjects($scope.subjectsPerPage, offset).then(function (response) {
            $scope.totalSubjects = response.data.Number;
            $scope.subjects = response.data.Elements;
        });
    };
})