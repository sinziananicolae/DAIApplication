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
    })

    .filter('showTime', function () {
        return function (input) {
            if (input !== undefined && input.toString().length === 1) {
                return "0" + input;
            }

            return input;
        };
    })

    .filter('showCreatedDate', function () {
        return function (input) {
            if (input !== undefined && input !== null) {
                return moment(new Date(input)).utc().format('DD MMMM YYYY hh:mm:ss A');
            }

            return "-";
        };
    })

    .filter('testTime', function () {
        return function (input) {
            if (input !== undefined && input !== null) {
                var minutes = parseInt(input / 60);
                var seconds = input - minutes * 60;

                return minutes + " minute(s) " + seconds + " second(s) ";
            }

            return "-";
        };
    })

    .filter('avgScore', function () {
        return function (input) {
            if (input !== undefined && input !== null) {
                var score = 0;
                _.each(input, function(test) {
                    score += test.Score;
                });

                return input.length ? score / input.length : 0;
            }

            return 0;
        };
    })

    .filter('avgTestTime', function () {
        return function (input) {
            if (input !== undefined && input !== null) {
                var time = 0;
                _.each(input, function(test) {
                    time += test.Time;
                });

                return input.length ? time / input.length : 0;
            }

            return 0;
        };
    });

}());