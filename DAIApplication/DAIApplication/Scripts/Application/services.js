(function () {
    "use strict";

    var serviceMetods = {
        get: { method: "GET" },
        create: { method: "POST" },
        update: { method: "PUT" },
        patch: { method: "PATCH" },
        remove: { method: "DELETE" }
    };

    angular.module("services", ["ngResource"])
        .factory("UserService", ["$resource", function ($resource) {
            return $resource("/api/user", {}, serviceMetods);
        }])
        .factory("QTypesService", ["$resource", function ($resource) {
            return $resource("/api/qTypes", {}, serviceMetods);
        }])
        .factory("CategoryService", ["$resource", function ($resource) {
            return $resource("/api/category", {}, serviceMetods);
        }])
        .factory("TestService", ["$resource", function ($resource) {
            return $resource("/api/test/:id", { id: "@id" }, serviceMetods);
        }])
        .factory("TestResultService", ["$resource", function ($resource) {
            return $resource("/api/test-result/:testId/:resultId", { testId: "@testId", resultId: "@resultId" }, serviceMetods);
        }]);

}());