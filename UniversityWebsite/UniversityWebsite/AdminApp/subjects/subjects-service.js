angular.module('configApp.subjects.service', [

])

.factory('Subjects', ['$resource', function ($resource) {
    return $resource('/api/subjects', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory("News", ['$resource', function($resource) {
        return $resource('/api/subjects/:subjectId/news/:id', {subjectId: '@id'}, {
            query: { method: 'GET', isArray: true },
            post: { method: 'POST' },
            update: { method: 'PUT' },
            remove: { method: 'DELETE' }
        });
}])

.factory("Students", ['$resource', function($resource) {
    return $resource('/api/subjects/:subjectId/students', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory("Teachers", ['$resource', function ($resource) {
    return $resource('/api/subjects/:subjectId/teachers', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory('subjectsService', ['$http', 'utils', 'Subjects', 'News', 'Students', 'Teachers',
    function ($http, utils, Subjects, News, Students, Teachers) {
    var path = "/api/subjects";

    var factory = {};

    factory.querySubjects = function (limit, offset) {
        return $http.get(path, { params: {limit: limit, offset: offset } });
    };

    factory.post = Subjects.post;
    factory.update = Subjects.update;

    factory.postNews = News.post;
    factory.updateNews = News.update;
    factory.removeNews = News.remove;

    factory.getStudents = Students.query;

    factory.getSignUpRequests = function(subjectId) {
        return $http.get('/api/signup', { params: { subjectId: subjectId } });
    };
    factory.approveSignUpRequests = function(ids) {
        return $http.post('/api/signup/approve', ids);
    };
    factory.rejectSignUpRequests = function (ids) {
        return $http.post('api/signup/reject', ids);
    };

    factory.queryTeachers = function(subjectId) {
        return Teachers.query({ subjectId: subjectId });
    };

    factory.updateTeachers = function (subjectId, teacherIds) {
        return Teachers.post({ subjectId: subjectId }, teacherIds);
    };

    return factory;
}]);