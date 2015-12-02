angular.module('configApp.pages.service', [

])

.factory('Pages', ['$resource', function ($resource) {
    var path = "/api/page"

    return $resource(path + '/:id', {}, {
        query: { method: 'GET' },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])
.factory('pages', ['$http', 'utils', function ($http, utils) {
    var path = "/api/page";
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

    var pages = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () {
        return pages;
    };

    factory.get = function (id) {
        return pages.then(function () {
            return utils.findByName(pages, id);
        })
    };


    return factory;
}]);