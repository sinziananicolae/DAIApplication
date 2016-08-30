(function () {
    "use strict";

    angular.module("app").service("StoreService", function () {

        this.save = function (user) {
            this.user = user;

        };

        this.getUserRole = function () {

            return this.user.Role;

        };

        this.getUser = function() {
            return this.user;
        }
    });

}());