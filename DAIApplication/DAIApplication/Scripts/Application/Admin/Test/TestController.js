(function () {
    "use strict";

    angular.module("app")
        .controller("TestCtrl", ["$scope", "toastr", "$routeParams", "QTypesService", "CategoryService", "TestService", testController]);

    function testController($scope, toastr, $routeParams, qTypesService, categoryService, testService) {
        $scope.answersIndexes = ["a", "b", "c", "d", "e", "f", "g"];
        $scope.singleAnswers = {};
        var removedAnswers = [];
        var removedQuestions = [];

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
                $scope.setSubcategories(test.Category.Id);

                _.each(test.Questions, function(question) {
                    addQuestion(question);
                });
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
            
            _.each(question.Answers, function(answer) {
                addAnswer(answer, q);
            });

            $scope.test.Questions.push(q);
        }

        function addAnswer(answer, q) {
            var a = {
                Answer: answer.Answer,
                Id: answer.Id,
                Guid: guid(),
                Correct: answer.Correct
            };
            q.Answers.push(a);

            if (q.QTypeId === 1 && answer.Correct) {
                $scope.singleAnswers[q.Guid] = a.Guid;
            }
        }

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
            if (question.Answers[index].Id) removedAnswers.push(question.Answers[index].Id);
            question.Answers.splice(index, 1);
        }

        $scope.saveTest = function () {
            var cannotSave = null;

            var objToSave = $scope.test;
            _.find(objToSave.Questions, function (question, index) {
                if (question.QTypeId === 1) {
                    if (!$scope.singleAnswers[question.Guid]) {
                        cannotSave = index + 1;
                        return true;
                    }
                    _.find(question.Answers,
                        function (answer) {
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

            if (objToSave.Id)
                editTest(objToSave);
            else
                saveTest(objToSave);
        }

        function saveTest(objToSave) {
            testService.save(objToSave, function (response) {
                toastr.success("You have successfully added a new test!");
                $scope.test = {
                    Questions: []
                };
                $scope.singleAnswers = {};
            });
        }

        function editTest(objToSave) {
            objToSave.RemovedAnswersIds = removedAnswers;
            testService.update(objToSave, function (response) {
                toastr.success("You have successfully added a new test!");
                $scope.test = {
                    Questions: []
                };
                $scope.singleAnswers = {};
            });
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