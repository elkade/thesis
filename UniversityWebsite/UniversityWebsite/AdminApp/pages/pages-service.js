angular.module('configApp.pages.service', [

])

.factory('Pages', ['$resource', function ($resource) {
    var path = "/api/page";
    return $resource(path + '/:id', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])
.factory('pagesService', ['$http', 'utils',  'Pages', function ($http, utils, Pages) {
    var path = "/api/page";

    var pages = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};

    factory.all = function () {
        return pages;
    };

    factory.query = Pages.query;
    factory.update = Pages.update;
    factory.post = Pages.post;
    factory.remove = Pages.remove;

    return factory;
}]);