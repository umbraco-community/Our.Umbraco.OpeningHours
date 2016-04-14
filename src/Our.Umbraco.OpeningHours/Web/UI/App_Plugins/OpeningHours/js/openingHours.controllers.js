(function () {
	angular.module("umbraco").controller("openingHours.controllers.OpeningHoursController", function ($scope) {

	    $scope.requireHolidayDates = $scope.model.config.requireHolidayDates;

	    var defaultHolidayDate = $scope.requireHolidayDates
	        ? new Date()
	        : null;

	    var datePickerConfig = { pickDate: true, pickTime: false, pick12HourFormat: false, format: "YYYY-MM-DD" };

	    var weekdayConfig = { label: "", day: 0, opens: "09:00", closes: "17:00", isOpen: true };
	    var holidayConfig = { label: "", date: { view: 'datepicker', config: datePickerConfig, value: defaultHolidayDate }, opens: "09:00", closes: "17:00", isOpen: true };

	    var weekDays = [
            { id: 1, name: "Monday" },
            { id: 2, name: "Tuesday" },
            { id: 3, name: "Wednesday" },
            { id: 4, name: "Thursday" },
            { id: 5, name: "Friday" },
            { id: 6, name: "Saturday" },
            { id: 0, name: "Sunday" }
	    ];

		$scope.createHolidayDay = function () {
		    var openingDay = angular.copy(holidayConfig);
		    $scope.model.data.holidays.push(openingDay);
		};

		$scope.deleteHolidayDay = function (day) {
		    var index = $scope.model.data.holidays.indexOf(day);
			$scope.model.data.holidays.splice(index, 1);
		};

	    var initData = function () {

	        $scope.model.data = {
	            weekdays: [],
	            holidays: []
	        };

	        if ($scope.model.value === "") {

	            // Populate default model
	            angular.forEach(weekDays, function (val, key) {
	                var model = angular.copy(weekdayConfig);

	                model.day = val.id;
	                model.label = val.name;

	                $scope.model.data.weekdays.push(model);
	            });

	        } else {

	            // Populate model from stored value
	            angular.forEach(weekDays, function (val, key) {
	                var model = angular.copy(weekdayConfig);

	                model.day = val.id;
	                model.label = val.name;

	                model.opens = $scope.model.value.weekdays[model.day].opens;
	                model.closes = $scope.model.value.weekdays[model.day].closes;
	                model.isOpen = $scope.model.value.weekdays[model.day].isOpen;

	                $scope.model.data.weekdays.push(model);
	            });

	            if ("holidays" in $scope.model.value) {
	                angular.forEach($scope.model.value.holidays, function(val, key) {
	                    var model = angular.copy(holidayConfig);

	                    model.label = val.label;

	                    model.date.value = val.date;
	                    model.opens = val.opens;
	                    model.closes = val.closes;
	                    model.isOpen = val.isOpen;

	                    $scope.model.data.holidays.push(model);
	                });
	            }

	        }
	    }

        var storeData = function() {
            
            var data = {
                weekdays: {},
                holidays: []
            };

            angular.forEach($scope.model.data.weekdays, function (val, key) {
                data.weekdays[val.day] = {
                    opens: val.opens,
                    closes: val.closes, 
                    isOpen: val.isOpen
                };
            });

            angular.forEach($scope.model.data.holidays, function (val, key) {
                data.holidays.push({
                    label: val.label,
                    date: val.date.value,
                    opens: val.opens,
                    closes: val.closes,  
                    isOpen: val.isOpen
                });
            });

            $scope.model.value = data;
        }

        initData();

        $scope.activeSubmitWatcher = 0;  
        $scope.submitWatcherOnLoad = function () {
            return ++$scope.activeSubmitWatcher; 
        }  
        $scope.submitWatcherOnSubmit = function () {  
            storeData();
        }

	});

	angular.module("umbraco").controller("openingHours.controllers.WeekdayOpeningHoursController", function ($scope) {

	    var weekdayConfig = { label: "", day: 0, opens: "09:00", closes: "17:00", isOpen: true };

	    var weekDays = [
            { id: 1, name: "Monday" },
            { id: 2, name: "Tuesday" },
            { id: 3, name: "Wednesday" },
            { id: 4, name: "Thursday" },
            { id: 5, name: "Friday" },
            { id: 6, name: "Saturday" },
            { id: 0, name: "Sunday" }
	    ];

	    var initData = function () {

	        $scope.model.data = {
	            weekdays: []
	        };

	        if ($scope.model.value === "") {

	            // Populate default model
	            angular.forEach(weekDays, function (val, key) {
	                var model = angular.copy(weekdayConfig);

	                model.day = val.id;
	                model.label = val.name;

	                $scope.model.data.weekdays.push(model);
	            });

	        } else {

	            // Populate model from stored value
	            angular.forEach(weekDays, function (val, key) {
	                var model = angular.copy(weekdayConfig);

	                model.day = val.id;
	                model.label = val.name;

	                model.opens = $scope.model.value[model.day].opens;
	                model.closes = $scope.model.value[model.day].closes;
	                model.isOpen = $scope.model.value[model.day].isOpen;

	                $scope.model.data.weekdays.push(model);
	            });

	        }
	    }

	    var storeData = function () {

	        var data = {};

	        angular.forEach($scope.model.data.weekdays, function (val, key) {
	            data[val.day] = {
	                opens: val.opens,
	                closes: val.closes,
	                isOpen: val.isOpen
	            };
	        });

	        $scope.model.value = data;
	    }

	    initData();

	    $scope.activeSubmitWatcher = 0;
	    $scope.submitWatcherOnLoad = function () {
	        return ++$scope.activeSubmitWatcher;
	    }
	    $scope.submitWatcherOnSubmit = function () {
	        storeData();
	    }

	});

	angular.module("umbraco").controller("openingHours.controllers.HolidayOpeningHoursController", function ($scope) {

	    $scope.requireHolidayDates = $scope.model.config.requireHolidayDates;

	    var defaultHolidayDate = $scope.requireHolidayDates
	        ? new Date()
	        : null;

	    var datePickerConfig = { pickDate: true, pickTime: false, pick12HourFormat: false, format: "YYYY-MM-DD" };
	    var holidayConfig = { label: "", date: { view: 'datepicker', config: datePickerConfig, value: defaultHolidayDate }, opens: "09:00", closes: "17:00", isOpen: true };

	    $scope.createHolidayDay = function () {
	        var openingDay = angular.copy(holidayConfig);
	        $scope.model.data.holidays.push(openingDay);
	    };

	    $scope.deleteHolidayDay = function (day) {
	        var index = $scope.model.data.holidays.indexOf(day);
	        $scope.model.data.holidays.splice(index, 1);
	    };

	    var initData = function () {

	        $scope.model.data = {
	            holidays: []
	        };

	        if ($scope.model.value !== "") {

	            // Populate model from stored value
	            angular.forEach($scope.model.value, function (val, key) {
	                var model = angular.copy(holidayConfig);

	                model.label = val.label;

	                model.date.value = val.date;
	                model.opens = val.opens;
	                model.closes = val.closes;
	                model.isOpen = val.isOpen;

	                $scope.model.data.holidays.push(model);
	            });

	        }
	    }

	    var storeData = function () {

	        var data = [];

	        angular.forEach($scope.model.data.holidays, function (val, key) {
	            data.push({
	                label: val.label,
	                date: val.date.value,
	                opens: val.opens,
	                closes: val.closes,
	                isOpen: val.isOpen
	            });
	        });

	        $scope.model.value = data;
	    }

	    initData();

	    $scope.activeSubmitWatcher = 0;
	    $scope.submitWatcherOnLoad = function () {
	        return ++$scope.activeSubmitWatcher;
	    }
	    $scope.submitWatcherOnSubmit = function () {
	        storeData();
	    }

	});

    angular.module("umbraco.directives")
        .directive('ohTimePicker', function () {
            return {
                scope: {
                    model: "="
                },
                restrict: 'E',
                replace: true,
                templateUrl: '/app_plugins/openinghours/views/openingHours.timePicker.html',
                link: function (scope, element, attrs, ctrl) {

                    scope.times = [];

                    for (var h2 = 0; h2 < 24; h2++) {
                        for (var m2 = 0; m2 < 12; m2++) {
                            var t = ("0" + h2).slice(-2) + ":" + ("0" + (m2*5)).slice(-2);
                            scope.times.push(t);
                        }
                    }

                }
            };
        });

    angular.module("umbraco.directives")
        .directive('ohSubmitWatcher', function ($rootScope) {
        var link = function (scope, element, attrs, ngModelCtrl) {
            // call the load callback on scope to obtain the ID of this submit watcher
            var id = scope.loadCallback();
            var unSubscribe = scope.$on("formSubmitting", function (ev, args) {
                // on the "formSubmitting" event, call the submit callback on scope to notify the controller to do it's magic
                if (id === scope.activeSubmitWatcher) {
                    scope.submitCallback();
                }
            });
        }

        return {
            restrict: "E",
            replace: true,
            template: "",
            scope: {
                loadCallback: '=',
                submitCallback: '=',
                activeSubmitWatcher: '='
            },
            link: link
        }
    });

}());

