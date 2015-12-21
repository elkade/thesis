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
.factory('filesService', ['$http', 'Files', function ($http, Files) {

    var factory = {};

    factory.allFiles = Files.query;
    factory.remove = Files.remove;

    return factory;
}]);