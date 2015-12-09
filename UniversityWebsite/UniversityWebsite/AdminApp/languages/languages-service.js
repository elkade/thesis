angular.module('configApp.languages.service', [
])

.factory('Languages', ['$resource', function($resource) {
    return $resource('/language/:lang', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory('languages', ['$http', 'utils', function ($http, utils) {
    var path = "/api/language";

    var languages = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var dictionaries = $http.get("/language/dictionaries").then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () {
        return languages;
    };

    factory.allDictionaries = function () {
        console.log(dictionaries);
        return dictionaries;
    };


    return factory;
}])

.factory('dictionaries', ['$http', 'utils', function ($http, utils) {
    var path = "/language/dictionaries";

    var dictionaries = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () {
        return dictionaries;
    };

    return factory;
}]);