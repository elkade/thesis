angular.module('configApp.menus.service', [
])

.factory('Menus', ['$resource', function($resource) {
    return $resource('api/menu/main/:lang', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory('menus', ['$http', 'utils', function ($http, utils) {
    var path = "/api/menu/main";

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