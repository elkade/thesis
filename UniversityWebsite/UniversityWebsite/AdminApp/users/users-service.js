angular.module('configApp.users.service', [

])

.factory('Users', ['$resource', function ($resource) {
    var path = "/api/users";

    return $resource(path + '/:id', {}, {
        query: { method: 'GET', isArray: true },
        post: { method: 'POST' },
        update: { method: 'PUT' },
        remove: { method: 'DELETE' }
    });
}])

.factory('userService', ['$http', 'utils', 'Users', function ($http, utils, Users) {
    var path = "/api/users/";

    var factory = {};

    factory.queryUsers = function (limit, offset) {
        return $http.get(path, { params: { limit: limit, offset: offset } });
    };

    factory.update = Users.update;
    factory.post = Users.post;

    return factory;
}]);