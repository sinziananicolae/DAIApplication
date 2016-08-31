(function () {
    "use strict";

    angular.module("app")
        .controller("TestCtrl", ["$scope", "toastr", "QTypesService", "CategoryService", "TestService", testController]);

    function testController($scope, toastr, qTypesService, categoryService, testService) {
        $scope.answersIndexes = ["a", "b", "c", "d", "e", "f", "g"];
        $scope.singleAnswers = {};

        $scope.test = {
            Questions: []
        };

        qTypesService.get(function (response) {
            $scope.qTypesList = response.data;
        });

        categoryService.get(function (response) {
            $scope.categories = response.data;
        });

        $scope.setSubcategories = function (categoryId) {
            $scope.subCategories = _.findWhere($scope.categories, { Id: categoryId }).Subcategories;
        };


        $scope.addQuestion = function () {
            var newGuid = guid();
            $scope.test.Questions.push({
                Text: "",
                QTypeId: $scope.selectedQType.Id,
                Guid: newGuid,
                Answers: [
                {
                    Answer: "",
                    Correct: false,
                    Guid: guid()
                },
                {
                    Answer: "",
                    Correct: false,
                    Guid: guid()
                },
                {
                    Answer: "",
                    Correct: false,
                    Guid: guid()
                }]
            });

            $scope.singleAnswers[newGuid] = "";
        };

        $scope.addAnswer = function (question) {
            question.Answers.push({
                Answer: "",
                Correct: false,
                Guid: guid()
            });
        }

        $scope.removeAnswer = function (question, index) {
            question.Answers.splice(index, 1);
        }

        $scope.saveTest = function () {
            var cannotSave = null;
            
            var objToSave = $scope.test;
            _.find(objToSave.Questions, function(question, index) {
                if (question.QTypeId === 1) {
                    if (!$scope.singleAnswers[question.Guid]) {
                        cannotSave = index + 1;
                        return true;
                    }
                    _.find(question.Answers,
                        function(answer) {
                            if (answer.Guid === $scope.singleAnswers[question.Guid]) {
                                answer.Correct = true;
                                return true;
                            }
                        });
                } else {
                    var correctAnswer = _.findWhere(question.Answers, { Correct: true });
                    if (!correctAnswer) {
                        cannotSave = index + 1;
                    }
                }
            });

            if (cannotSave) {
                toastr.error("Please select a correct answer for question " + cannotSave + " and retry saving the test!");
                return;
            }

            testService.save(objToSave, function (response) { });
        }

        function guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }

            var newGuid = s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();

            return newGuid;
        }
    }
}());