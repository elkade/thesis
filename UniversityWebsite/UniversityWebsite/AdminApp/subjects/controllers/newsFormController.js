angular.module('configApp.subjects')

.controller('newsFormCtrl', function ($scope, $sce, subjectsService, utils) {

    $scope.newsMaxLength = 200;
    $scope.init = function(news) {
        $scope.formHide = true;
        if (news.Id == null) {
            $scope.formHide = false;
        }
    };

    $scope.updateNews = function (subject, news) {
        utils.showValidation($scope.form);
        if ($scope.form.$valid) {
            if (news.Id != null) {
                subjectsService.updateNews({ subjectId: subject.Id, id: news.Id }, news, function (response) {
                    console.log(response);
                    news.Id = response.Id;
                    news.Content = response.Content;
                    news.Header = response.Header;
                    $scope.formHide = true;
                }, errorHandler);
                $scope.formHide = true;
            } else {
                subjectsService.postNews({ subjectId: subject.Id }, news, function (response) {
                    console.log(response);
                    news.Id = response.Id;
                    news.Content = response.Content;
                    news.Header = response.Header;
                    $scope.formHide = true;
                }, errorHandler);
            }
        }
    };

    $scope.edit = function() {
        $scope.formHide = false;
    };

    $scope.hideForm = function() {
        $scope.formHide = true;
    };

    $scope.remove = function(subject, news) {
        subjectsService.removeNews({ newsId: news.Id });
        utils.removeByHeader(subject.News, news.Header);
    };

    $scope.html = function (content) {
        return $sce.trustAsHtml(content);
    };

    var errorHandler = function (response) {
        $scope.errors = utils.parseErrors(response.data.ModelState);
    };
})