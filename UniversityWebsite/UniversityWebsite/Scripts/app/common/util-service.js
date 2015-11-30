﻿angular.module('configApp.utils.service', [

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
        findByTitle: function findByTitle(a, title) {
            for (var i = 0; i < a.length; i++) {
                if (a[i].Title == title) return a[i];
            }
            return null;
        },

        findByCountryCode: function findByCountryCode(a, countryCode) {
            for (var i = 0; i < a.length; i++) {
                if (a[i].CountryCode == countryCode) return a[i];
            }
            return null;
        },

        removeByTitle: function removeByTitle(a, title) {
            for (var i = a.length - 1; i >= 0; i--) {
                if (a[i].Title == title) {
                    a.splice(i, 1);
                }
            }
        },


        parseErrors: function parseErrors(modelState) {
            var errors = [];
            for (var key in modelState) {
                for (var i = 0; i < modelState[key].length; i++) {
                    errors.push(modelState[key][i]);
                }
            }
            return errors;
        }
    };
});