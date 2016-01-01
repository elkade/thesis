angular.module('configApp.pages.service', [

])

.factory('Pages', ['$resource', function ($resource) {
    var path = "/api/page";
    return $resource(path + '/:id', {}, {
        query: { method: 'GET'},
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])
.factory('pagesService', ['$http', 'utils',  'Pages', function ($http, utils, Pages) {
    var path = "/api/page";

    //var pages = $http.get(path).then(function (resp) {
    //    return resp.data;
    //});

    var factory = {};

    //factory.all = function () {
    //    return pages;
    //};

    factory.update = Pages.update;
    factory.post = Pages.post;
    factory.remove = Pages.remove;

    factory.queryPages = function (limit, offset) {
        return $http.get(path, { params: { limit: limit, offset: offset } });
    };

    factory.findPage = function (pageId) {
        return Pages.query({ id: pageId });
    };

    return factory;
}]);