angular.module('configApp.subjects.service', [

])

.factory('Subjects', ['$resource', function ($resource) {
    return $resource('/api/teaching/subjects', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory("News", ['$resource', function($resource) {
        return $resource('/api/teaching/subjects/:subjectId/news/:id', {subjectId: '@id'}, {
            query: { method: 'GET', isArray: true },
            post: { method: 'POST' },
            update: { method: 'PUT' },
            remove: { method: 'DELETE' }
        });
}])

.factory("Students", ['$resource', function($resource) {
    return $resource('/api/teaching/subjects/:subjectId/students', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory('subjectsService', ['$http', 'utils', 'Subjects', 'News', 'Students',
    function ($http, utils, Subjects, News, Students) {
    var path = "/api/teaching/subjects";

    var subjects = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () {
        return subjects;
    };

    factory.get = function (id) {
        return subjects.then(function() {
            return utils.findByName(subjects, id);
        });
    };

    factory.post = Subjects.post;
    factory.update = Subjects.update;

    factory.postNews = News.post;
    factory.updateNews = News.update;
    factory.removeNews = News.remove;

    factory.getStudents = Students.query;

    return factory;
}]);