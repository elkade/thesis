angular.module('configApp.main-pages.service', [
])

.factory('MainPages', ['$resource', function ($resource) {
    return $resource('api/tile/:lang', {}, {
        query: { method: 'GET', isArray: true},
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory('mainPages', ['$http', 'utils', function ($http, utils) {
    var path = "/api/tile";

    var menus = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () { 
        return menus;
    };

    factory.get = function (lang) {
        return menus.then(function () {
            return utils.findByCountryCode(menus, lang);
        })
    };


    return factory;
}]);