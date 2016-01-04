angular.module('configApp.subjects')

.controller('subjectSignUpRequestCtrl', function ($scope, subjectsService) {
    $scope.totalRequests = 0;
    $scope.currentPage = 1;
    $scope.requestsPerPage = 8;

    getPage(1);

    $scope.pageChanged = function (newPage) {
        getPage(newPage);
    };

    $scope.pagination = {
        current: 1
    };

    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.requestsPerPage;
        subjectsService.querySignUpRequests($scope.subject.Id, $scope.requestsPerPage, offset).then(function (response) {
            $scope.totalRequests = response.data.Number;
            $scope.requests = response.data.Elements;
        });
    };

    $scope.approve = function() {
        var selectedRequests = getSelectedRequestIds($scope.requests);
        if (selectedRequests.length > 0) {
            subjectsService.approveSignUpRequests(selectedRequests).then(function (response) {
                var alert = { type: 'success', msg: 'Selected students were successfully added to the subject.' };
                $scope.addAlert(alert);
                getPage(1);
            }, function (error) {
                var alert = { type: 'alert', msg: 'Error: ' + error };
                $scope.addAlert(alert);
            });
        }
    };

    $scope.reject = function() {
        var selectedRequests = getSelectedRequestIds($scope.requests);
        if (selectedRequests.length > 0) {
            subjectsService.rejectSignUpRequests(selectedRequests).then(function (response) {
                var alert = { type: 'success', msg: 'Requsts from selected students were successfully rejected.' };
                $scope.addAlert(alert);
                getPage(1);
            }, function (error) {
                var alert = { type: 'alert', msg: 'Error: ' + error };
                $scope.addAlert(alert);
            });
        }
    };

    function getSelectedRequestIds(signUpRequests) {
        return Enumerable.From(signUpRequests)
            .Where(function (request) { return request.selected; })
            .Select(function (request) { return request.Id; })
            .ToArray();
    };

});