angular.module('configApp.subjects.service', [

])

.factory('subjects', ['$http', 'utils', function ($http, utils) {
    var path = "/api/subjects";
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

    var subjects = $http.get(path).then(function (resp) {
        return resp.data;
    });

    var factory = {};
    factory.all = function () {
        return subjects;
    };

    factory.get = function (id) {
        return subjects.then(function () {
            return utils.findByName(subjects, id);
        })
    };

    return factory;
}]);