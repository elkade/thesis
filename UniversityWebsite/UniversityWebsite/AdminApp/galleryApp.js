angular.module('galleryApp', [
    'mm.foundation',
    'angularUtils.directives.dirPagination',
    'ngAnimate',
    'angularSpinner',
    'configApp.gallery',
    'configApp.utils.service',
    'configApp.files.service',
    'ngResource'])

.directive('ncgRequestVerificationToken', ['$http', function ($http) {
    return function (scope, element, attrs) {
        $http.defaults.headers.common['RequestVerificationToken'] = attrs.ncgRequestVerificationToken || "no request verification token";
    };
}])

