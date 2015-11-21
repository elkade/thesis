angular.module('configApp.menus.service', [

])

.factory('menus', ['$http', 'utils', function ($http, utils) {
    var path = "/api/menus";
    //var http = function() {
    //    return $http({
    //        method: "GET",
    //        url: path,
    //        headers: { 'Content-Type': 'application/json' }
    //    });
    //}

    //this.pages = http().then(function (resp) {
    //    return resp.data;
    //});

    var menus = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () {
        return menus;
    };

    factory.get = function (id) {
        return menus.then(function () {
            return utils.findById(menus, id);
        })
    };


    return factory;
}]);