angular.module("umbraco").controller("OpeningHours.Controllers.OpeningHoursController", function ($scope) {

    // Double-check that we're getting the model back in the expected format 
    // (ie.Nested Content might init the property editor with a string)
    if (typeof ($scope.model.value) === 'string') {
        if ($scope.model.value === '') {
            $scope.model.value = {};
        } else {
            $scope.model.value = JSON.parse($scope.model.value);
        }
    }

    function parseBoolean(str) {
        str = str + '';
        return str == '1' || str == 'true';
    }

    // Ensure properties of the configuration object
    $scope.model.config.hideWeekdays = parseBoolean($scope.model.config.hideWeekdays);
    $scope.model.config.hideHolidays = parseBoolean($scope.model.config.hideHolidays);
    
    // Option to hide property label
    $scope.model.hideLabel = parseBoolean($scope.model.config.hideLabel);
}); 