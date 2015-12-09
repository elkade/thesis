﻿angular.module('configApp.languages.service', [
])

.factory('Languages', ['$resource', function($resource) {
    return $resource('/api/languages/:lang', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory('Dictionaries', ['$resource', function ($resource) {
    return $resource('/api/languages/dictionaries', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])


.factory('dictionaries', ['$http', 'utils', function ($http, utils) {
    var path = "/api/languages/dictionaries";
    var keysPath = "/api/languages/keys";

    var dictionaries = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var translationKeys = $http.get(keysPath).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () {
        return dictionaries;
    };

    factory.allTranslationKeys = function () {
        return translationKeys;
    };

    return factory;
}])

.factory('languageService', ['$http', 'utils', function ($http, utils) {
    var dictionariesPath = "/api/languages/dictionaries";
    var keysPath = "/api/languages/keys";
    var languagesPath = "/api/languages";

    var dictionaries = $http.get(dictionariesPath).then(function (resp) {
        return resp.data;
    });

    var translationKeys = $http.get(keysPath).then(function (resp) {
        return resp.data;
    });

    var languages = $http.get(languagesPath).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.allDictionaries = function () {
        return dictionaries;
    };

    factory.allTranslationKeys = function () {
        return translationKeys;
    };

    factory.allLanguages = function () {
        return languages;
    };

    factory.refresh = function() {
        languages = $http.get(languagesPath).then(function(resp) {
            return resp.data;
        });
    };

    return factory;
}]);