(function () {
    'use strict';

    angular.module('filters', [])

    .filter('capitalize', function () {
        return function (input, scope) {
            var words = input.split(' ');
            var result = [];
            words.forEach(function (word) {
                result.push(word.charAt(0).toUpperCase() + word.slice(1).toLowerCase());
            });
            return result.join(' ');
        }
    })

    .filter('notext', function () {
        return function (input) {
            if (input === undefined || input === null || input === "") {
                return "-";
            }

            return input;
        };
    });

}());