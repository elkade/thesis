angular.module('configApp.gallery.service', [
])

.factory('imagesService', ['$http', 'utils',
    function ($http, utils) {

        var factory = {};

        factory.getGallery = function(limit, offset) {
            return $http.get("/api/file/gallery", { params: { limit: limit, offset: offset } });
        };

        factory.upload = function (image) {
            var fd = new FormData();
            fd.append('file', image);

            return $http.post('/api/file/gallery', fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            });
        };

        factory.remove = function (fileId) {
            return $http.delete('/api/file/' + fileId, fileId);
        };

        return factory;
    }]);