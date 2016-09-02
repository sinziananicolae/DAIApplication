(function () {
    "use strict";

    angular.module("app")
        .controller("AdminTestSummaryCtrl", ["$scope", "toastr", "$routeParams", "$location", "AdminTestService", testController]);

    function testController($scope, toastr, $routeParams, $location, testService) {
        $scope.answersIndexes = ["a", "b", "c", "d", "e", "f", "g"];
        $scope.resultId = $routeParams.resultId;

        testService.get({ testId: $routeParams.testId  }, function (response) {
            $scope.test = response.data.Test;
            $scope.testInfo = response.data.TestInfo;
        });
    }
}());