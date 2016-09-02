(function () {
    "use strict";

    angular.module("app")
        .controller("UserProfileCtrl", ["$scope", "toastr", "$routeParams", "$location", "UserProfileService", testController]);

    function testController($scope, toastr, $routeParams, $location, userProfileService) {
        $scope.loading = false;

        userProfileService.get(function (response) {
            $scope.userProfile = response.data;
        });

        $scope.saveUserProfile = function () {
            $scope.loading = true;
            userProfileService.update($scope.userProfile, function (response) {
                $scope.loading = false;
                if (response.success) {
                    toastr.success("User Profile successfully updated!");
                }
            });
        };
    }
}());