angular.module('configApp.gallery', ['ui.router', 'configApp.gallery.service'])

    .config(
    [
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            var getGallery = function (imagesService) {
                return imagesService.getGallery();
            };

            $stateProvider
                .state('gallery', {
                    url: '/gallery',
                    templateUrl: 'adminapp/views/gallery/gallery.html',
                    params: {
                        multipleSelection: true
                    },
                    controller: 'galleryCtrl'
                });
        }
    ]);