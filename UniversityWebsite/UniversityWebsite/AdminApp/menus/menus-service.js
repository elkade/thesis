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

.factory('menuService', ['$http', 'utils', 'Menus', function ($http, utils, Menus) {
    var path = "/api/menu/main";

    var menus = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.allMenus = function () {
        return menus;
    };

    factory.get = function (lang) {
        return menus.then(function() {
            return Enumerable.From(menus).FirstOrDefault(function(menu) { return menu.CountryCode == lang; });
        });
    };

    factory.update = Menus.update;


    return factory;
}]);