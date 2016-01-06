configApp.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function(scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            
            element.bind('change', function(){
                scope.$apply(function(){
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

configApp.directive('ncgRequestVerificationToken', [
    '$http', function($http) {
        return function(scope, element, attrs) {
            $http.defaults.headers.common['RequestVerificationToken'] = attrs.ncgRequestVerificationToken || "no request verification token";
        };
    }
]);

configApp.directive('pageList', function() {
    return {
        restrict: "E",
        templateUrl: "/adminapp/views/pages/pages.list.html",
        scope: {
            selected: '&',
            changed: '&',
            showTitle: '=',
        },
        controller: function ($scope, pagesService) {
            $scope.totalPages = 0;
            $scope.currentPage = 1;
            $scope.pagesPerPage = 8;
            $scope.actionAvailable = $scope.selected() != null;

            getPage(1);

            $scope.pageChanged = function (newPage) {
                getPage(newPage);
            };

            $scope.pagination = {
                current: 1
            };

            function getPage(pageNumber) {
                var offset = (pageNumber - 1) * $scope.pagesPerPage;
                pagesService.queryPages($scope.pagesPerPage, offset).then(function (response) {
                    $scope.totalPages = response.data.Number;
                    $scope.pages = response.data.Elements;
                    $scope.changed()($scope.pages);
                });
            };

            $scope.select = function(page) {
                $scope.selected()(page);
            };
        }
    };
});

configApp.directive('userList', function () {
    return {
        restrict: "E",
        templateUrl: "/adminapp/views/users/user.list.html",
        scope: {
            showTitle: '=',
            role: '=',
            doubleClick: '&'
        },
        controller: function ($scope, userService) {
            $scope.totalUsers = 0;
            $scope.currentPage = 1;
            $scope.usersPerPage = 8;
            
            getPage(1);

            $scope.pageChanged = function (newPage) {
                getPage(newPage);
            };

            $scope.pagination = {
                current: 1
            };

            function getPage(pageNumber) {
                var offset = (pageNumber - 1) * $scope.usersPerPage;
                userService.queryUsers($scope.role, $scope.usersPerPage, offset).then(function (response) {
                    console.log(response);
                    $scope.totalUsers = response.data.Number;
                    $scope.users = response.data.Elements;
                });
            };

            $scope.select = function (user) {
                $scope.doubleClick()(user);
            };
        }
    };
});
