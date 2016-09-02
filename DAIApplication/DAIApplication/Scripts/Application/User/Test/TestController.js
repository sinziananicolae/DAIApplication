(function () {
    "use strict";

    angular.module("app")
        .controller("TakeTestCtrl", ["$scope", "toastr", "$routeParams", "$location", "QTypesService", "CategoryService", "TestService", testController]);

    function testController($scope, toastr, $routeParams, $location, qTypesService, categoryService, testService) {
        var interval;

        $scope.answersIndexes = ["a", "b", "c", "d", "e", "f", "g"];
        $scope.singleAnswers = {};

        $scope.test = {
            Questions: []
        };

        var startCountDown = function () {
            $scope.remainingMinutes = $scope.test.Time;
            $scope.remainingSeconds = 0;

            interval = setInterval(function () {
                if ($scope.remainingSeconds === 0) {

                    if ($scope.remainingMinutes === 0) {
                        $scope.testIsOver = true;
                        clearInterval(interval);
                        $scope.$apply();

                        if (confirm("Time is up! Do you want to submit the test?")) {
                            $scope.submitTest();
                        } else {
                            $location.path("/user-dashboard");
                        }
                        return;
                    }

                    $scope.remainingMinutes -= 1;
                    $scope.remainingSeconds = 59;
                } else {
                    $scope.remainingSeconds -= 1;
                }
                $scope.$apply();
            }, 1000);
        }

        if ($routeParams.id) {
            testService.get({ id: $routeParams.id }, function (response) {
                var test = response.data;
                $scope.test = {
                    Id: test.Id,
                    Name: test.Name,
                    QCategoryId: test.Category.Id,
                    QSubcategoryId: test.Subcategory.Id,
                    Time: test.Time,
                    Questions: []
                };

                _.each(_.shuffle(test.Questions), function (question) {
                    addQuestion(question);
                });

                startCountDown();
            });
        }

        function addQuestion(question) {
            var q = {
                Text: question.Text,
                QTypeId: question.QTypeId,
                Guid: guid(),
                Answers: [],
                Id: question.Id
            };

            _.each(_.shuffle(question.Answers), function (answer) {
                addAnswer(answer, q);
            });

            $scope.test.Questions.push(q);
        }

        function addAnswer(answer, q) {
            var a = {
                Answer: answer.Answer,
                Id: answer.Id,
                Guid: guid()
            };
            q.Answers.push(a);
        }

        $scope.submitTest = function () {
            clearInterval(interval);

            var objToSend = {
                TestId: $scope.test.Id,
                Time: $scope.test.Time * 60 - $scope.remainingMinutes * 60 - $scope.remainingSeconds,
                Answers: {}
            };

            _.each($scope.test.Questions,
                function (question) {
                    if (question.QTypeId === 1) {
                        if (!$scope.singleAnswers[question.Guid]) {
                            objToSend.Answers[question.Id] = [];
                            return true;
                        }
                        _.find(question.Answers,
                            function (answer) {
                                if (answer.Guid === $scope.singleAnswers[question.Guid]) {
                                    objToSend.Answers[question.Id] = [answer.Id];
                                    return true;
                                }
                            });
                    } else {
                        objToSend.Answers[question.Id] = [];
                        _.each(question.Answers,
                            function (answer) {
                                if (answer.Correct) {
                                    objToSend.Answers[question.Id].push(answer.Id);
                                    return true;
                                }
                            });
                    }
                });

            testService.save({ id: $routeParams.id }, objToSend, function (response) {
                if (response.success) {
                    $location.path("/test-summary/" + $scope.test.Id + "/" + response.data.Id);
                }
            });
    };

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