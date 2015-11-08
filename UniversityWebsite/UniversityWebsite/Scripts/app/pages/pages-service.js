﻿angular.module('configApp.pages.service', [

])

.factory('pages', ['$http', 'utils', function ($http, utils) {
    var path = "/api/pages";
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