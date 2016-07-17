angular.module("umbraco").controller("OpeningHours.Controllers.OpeningHoursController", function ($scope) {

    function parseBoolean(str) {
        str = str + '';
        return str == '1' || str == 'true';
    }

    // Ensure properties of the configuration object
    $scope.model.config.hideWeekdays = parseBoolean($scope.model.config.hideWeekdays);
    $scope.model.config.hideHolidays = parseBoolean($scope.model.config.hideHolidays);

}); 