angular.module('configApp.subjects')

.controller('subjectSignUpRequestCtrl', function ($scope, subjectsService) {

    loadSignUpRequsts();

    function loadSignUpRequsts() {
        subjectsService.getSignUpRequests($scope.subject.Id)
            .then(function (data) {
            $scope.signUpRequests = data.signUpRequests;
        });
    };

    $scope.approve = function() {
        var selectedRequests = getSelectedRequestIds($scope.signUpRequests);

        subjectsService.approveSignUpRequests(selectedRequests).then(function(response) {
            loadSignUpRequsts();
        }, function(error) {
            console.log("error");
        });
    };

    $scope.reject = function() {
        var selectedRequests = getSelectedRequestIds($scope.signUpRequests);
        subjectsService.rejectSignUpRequests(selectedRequests).then(function (response) {
            loadSignUpRequsts();
        }, function (error) {
            console.log("error");
        });
    };

    function getSelectedRequestIds(signUpRequests) {
        return Enumerable.From($scope.signUpRequests)
            .Where(function (request) { return request.selected; })
            .Select(function (request) { return request.Id; })
            .ToArray();
    };

});