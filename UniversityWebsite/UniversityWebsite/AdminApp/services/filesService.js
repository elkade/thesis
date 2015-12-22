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

    var factory = {};

    factory.allFiles = Files.query;
    factory.remove = Files.remove;

    factory.upload = function(subjectId, file) {
        return Upload.upload({
            url: 'api/file?subjectId=' + subjectId,
            data: { file: file }
        });
    };

    return factory;
}]);