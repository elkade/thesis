angular.module('configApp.main-pages.service', [
])

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