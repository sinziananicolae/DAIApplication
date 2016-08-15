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
        .factory("HiveFileService",
        [
            "$resource", "$http", function ($resource, $http) {
                $http.defaults.useXDomain = true;
                return $resource("/hivefiles/:hiveId/:type",
                    { hiveId: "@hiveId", type: "@type" },
                    serviceMetods);
            }
        ]);

}());