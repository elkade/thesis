angular.module('configApp.gallery')


.controller('galleryCtrl', function ($scope, $stateParams, $timeout, $q, imagesService) {
    $scope.totalImages = 0;
    $scope.currentPage = 1;
    $scope.imagesPerPage = 8;

    getPage(1);

    $scope.pageChanged = function (newPage) {
        getPage(newPage);
    };

    $scope.pagination = {
        current: 1
    };

    $scope.uploadImage = function () {
        var file = $scope.newImage;
        imagesService.upload(file).success(function(response) {
            getPage(1);
        }).error(function(error) {
            console.log(error);
        });

    };

    $scope.removeImages = function () {
        var selectedImages = Enumerable.From($scope.images).Where(function (image) { return image.selected; }).ToArray();
        var promises = [];
        Enumerable.From(selectedImages).ForEach(function (image) {
            var promise = imagesService.remove(image.Id);
            promises.push(promise);
        });

        $q.all(promises).then(function() {
            getPage(1);
        });
    };

    function getPage(pageNumber) {
        var offset = (pageNumber - 1) * $scope.imagesPerPage;
        imagesService.getGallery($scope.imagesPerPage, offset).then(function (response) {
            $scope.totalImages = response.data.Number;
            $scope.images = response.data.Elements;
        });
    };

    $scope.imgClicked = function (image) {
        if (image.selected == null) {
            image.selected = false;
        }
        image.selected = !image.selected;
    };
})