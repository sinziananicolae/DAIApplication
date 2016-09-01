(function () {
    "use strict";

    angular.module("app")
        .controller("UserDashboardCtrl", ["$scope", "$location", "CategoryService", "TestService", homeController]);

    function homeController($scope, $location, categoryService, testService) {
        var getCategories = function () {
            categoryService.get(function (response) {
                $scope.categories = response.data;

                _.each($scope.categories, function(category) {
                    var testsInCateg = _.filter($scope.tests, function(test) { return test.CategoryId === category.Id; });
                    category.TestsNo = testsInCateg ? testsInCateg.length : 0;

                    _.each(category.Subcategories, function (subCategory) {
                        var testsInSubCateg = _.filter($scope.tests, function (test) { return test.SubcategoryId === subCategory.Id; });
                        subCategory.TestsNo = testsInSubCateg ? testsInSubCateg.length : 0;
                    });
                });
            });
        };

        testService.get({ id: "all" }, function (response) {
            $scope.tests = response.data;
            $scope.filteredTests = angular.copy($scope.tests);

            getCategories();
        });
        
        $scope.selectCategory = function (category) {
            $scope.subCategories = _.findWhere($scope.categories, { Id: category.Id }).Subcategories;
            $scope.selectedCategory = category;
            $scope.selectedSubCategory = null;
            $scope.filteredTests = _.filter($scope.tests, function (test) { return test.CategoryId === category.Id; });
        };

        $scope.selectSubcategory = function (subCategory) {
            $scope.selectedSubCategory = subCategory;
            $scope.filteredTests = _.filter($scope.tests, function (test) { return test.SubcategoryId === subCategory.Id; });
        };

        $scope.clearCategoryFilter = function() {
            $scope.selectedCategory = null;
            $scope.selectedSubCategory = null;
            $scope.filteredTests = angular.copy($scope.tests);
        }

        $scope.takeTest = function(test) {
            $location.path("/take-test/" + test.Id);
        }
    }
}());