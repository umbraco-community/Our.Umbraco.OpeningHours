angular.module("umbraco").controller("OpeningHours.Controllers.OpeningHoursController", function ($scope) {

    function parseBoolean(str) {
        str = str + '';
        return str == '1' || str == 'true';
    }

    // Ensure properties of the configuration object
    $scope.model.config.disableWeekdays = parseBoolean($scope.model.config.disableWeekdays);
    $scope.model.config.disableHolidays = parseBoolean($scope.model.config.disableHolidays);

}); 