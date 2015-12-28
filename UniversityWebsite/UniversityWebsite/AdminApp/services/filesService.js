angular.module('configApp.files.service', [

])

.factory('Files', ['$resource', function ($resource) {
    var path = "/api/file/:fileId";
    return $resource(path, {subjectId: '@id'}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])
.factory('filesService', ['$http', 'Files', 'Upload', function ($http, Files, Upload) {
    var path = "/api/file";
    var factory = {};

    factory.remove = function(fileId) {
        return Files.remove({ fileId: fileId });
    };

    factory.upload = function(subjectId, file) {
        return Upload.upload({
            url: 'api/file?subjectId=' + subjectId,
            data: { file: file }
        });
    };

    factory.queryFiles = function (subjectId, limit, offset) {
        return $http.get(path, { params: { subjectId: subjectId, limit: limit, offset: offset } });
    };

    return factory;
}]);