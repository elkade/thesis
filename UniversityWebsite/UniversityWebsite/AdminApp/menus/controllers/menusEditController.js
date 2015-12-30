angular.module('configApp.menus')

.controller('menusEditCtrl', function ($scope, $state, menus, utils, menuService) {
    $scope.menus = menus;
    //$scope.pages = pages;
    $scope.alerts = [];

    $scope.activeMenu = menus[0];

    $scope.leftBar = { name: 'pages', url: 'adminapp/views/partials/pages.list.html' };
    $scope.menuEdit = { name: 'menuEdit', url: 'adminapp/views/menus/menu.edit.html' };
    $scope.menuList = { name: 'menuList', url: 'adminapp/views/partials/menu.list.html' };

    $scope.sortItems = function (items) {
        return items.sort(function (first, second) {
            return first.Order - second.Order;
        });
    },

    $scope.addToMenu = function (page) {
        var menuItem = new Object();
        menuItem.Order = $scope.activeMenu.Items.length;
        menuItem.Title = page.Title;
        menuItem.PageId = page.Id;

        $scope.activeMenu.Items.push(menuItem);
    },

    $scope.moveUp = function (menuItem, items) {
        items = $scope.sortItems(items);
        menuItem.Order--;
        items[menuItem.Order].Order++;
    },

    $scope.moveDown = function (menuItem, items) {
        items = items.sort(function (first, second) {
            return first.Order - second.Order;
        });
        menuItem.Order++;
        items[menuItem.Order].Order--;
    },

    $scope.remove = function (menuItem, items) {
        items = $scope.sortItems(items);
        utils.remove(items, menuItem);
        for (var i = 0; i < items.length; i++) {
            items[i].Order = i;
        }
    },

    $scope.update = function (menu) {
        menuService.update({ lang: menu.CountryCode }, menu, function (response) {
            var alert = { type: 'success', msg: 'The ' + menu.CountryCode + ' menu has been updated.' };
            $scope.addAlert(alert);
        }, errorHandler);
    },

    $scope.removeAll = function () {
        $scope.activeMenu.Items = [];
    },

    $scope.menuChanged = function (menu) {
        $scope.activeMenu = menu;
    },

    $scope.pagesFilterExpression = function (menu) {
        return menu.CountryCode == $scope.activeMenu.CountryCode;
    },

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.addAlert = function (alert) {
        $scope.alerts.push(alert);
    };

    var errorHandler = function(response) {
        var alert = { type: 'error', msg: 'Error: The ' + menu.CountryCode + ' menu cannot be updated.' };
        $scope.addAlert(alert);
    };

})