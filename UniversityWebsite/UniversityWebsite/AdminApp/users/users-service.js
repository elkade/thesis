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

    var users = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.allUsers = function () {
        return users;
        //return [
        //    {
        //        Id: '1',
        //        FirstName: 'Jan',
        //        LastName: 'Kowalski',
        //        Login: 'jkowalski',
        //        Email: 'jkowalski@kkk.pl',
        //        OwnedSubjects: [{Name: 'Algebra'}, {Name: 'Analiza'}],
        //        ParticipatedSubjects: [{Name: 'Algebra 2', Id: 1}, {Name: 'Analiza 2', Id: 2}],
        //    },
        //    {
        //        Id: '2',
        //        FirstName: 'Anna',
        //        LastName: 'Nowak',
        //        Login: 'anowak',
        //        Email: 'anowak@kkk.pl'
        //    },
        //];
    };

    factory.update = Users.update;
    factory.post = Users.post;

    return factory;
}]);