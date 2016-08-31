(function () {
    "use strict";

    angular.module("app")
        .controller("TestCtrl", ["$scope", "QTypesService", testController]);

    function testController($scope, qTypesService) {
        $scope.answersIndexes = ["a", "b", "c", "d", "e", "f", "g"];

        $scope.test = {
            questions: []
        };

        qTypesService.get(function (response) {
            $scope.qTypesList = response.data;
        });

        $scope.addQuestion = function () {
            $scope.test.questions.push({
                name: "",
                qType: $scope.selectedQType.Id,
                answers: [
                {
                    answer: "",
                    correct: false
                },
                {
                    answer: "",
                    correct: false
                },
                {
                    answer: "",
                    correct: false
                }]
            });
        };

        $scope.addAnswer = function(question) {
            question.answers.push({
                answer: "",
                correct: false
            });
        }

        $scope.removeAnswer = function(question, index) {
            question.answers.splice(index, 1);
        }
    }
}());