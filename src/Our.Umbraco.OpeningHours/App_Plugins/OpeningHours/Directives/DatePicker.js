angular.module('umbraco.directives').directive('ohDatePicker', function () {
    return {
        scope: {
            model: '='
        },
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/OpeningHours/Views/openingHours.datePicker.html',
        link: function (scope) {

            // Initialize the configuration of Umbraco's date picker
            scope.datePicker = {
                view: 'datepicker',
                config: { pickDate: true, pickTime: false, pick12HourFormat: false, format: 'YYYY-MM-DD' },
                value: scope.model
            };

            // Watch the value of the date picker so we can put it back into "model"
            scope.$watch(function () {
                return scope.datePicker.value;
            }, function (value) {
                scope.model = value;
            });

        }
    };
});