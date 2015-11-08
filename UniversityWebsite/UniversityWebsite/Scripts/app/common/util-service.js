angular.module('configApp.utils.service', [

])

.factory('utils', function () {
    return {
        findById: function findById(a, id) {
            for (var i = 0; i < a.length; i++) {
                if (a[i].id == id) return a[i];
            }
            return null;
        },
        findByName: function findByName(a, name) {
            for (var i = 0; i < a.length; i++) {
                if (a[i].Name == name) return a[i];
            }
            return null;
        },
    };
});