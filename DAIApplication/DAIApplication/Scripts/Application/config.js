(function () {
    "use strict";

    angular.module("app").service("StoreService", function () {

        var user = {};

        this.save = function (user) {
            this.user = user;

        };

        this.getUserRole = function () {

            return user.Role;

        };
    });

}());