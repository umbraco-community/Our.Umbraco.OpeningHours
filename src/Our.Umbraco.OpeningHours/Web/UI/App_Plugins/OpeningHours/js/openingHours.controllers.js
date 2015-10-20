(function () {
	angular.module("umbraco").controller("openingHours.controller", function ($scope, assetsService) {

		// re-use the date time picker config
		var dateTimePickerConfig = { pickDate: false, pickTime: true, pick12HourFormat: false, time24h: true, format: "HH:mm" };

		if ($scope.model.value === "") {
			// Set up the model
			$scope.model.value = {
				weekDays: [],
				lastSundayInMonth: {
					isActive: false,
					open: {},
					closed: {}
				},
				firstSundayInMonth: {
					isActive: false,
					open: {},
					closed: {}
				},
				specificDays: []
			};

		    

			// Add week days
			// TODO: Need to find a way to not store the entire Umbraco datetime picker objects in the cache since 1) it's not needed and 2) it's ugly as h... ;-)		
		    $scope.model.value.weekDays.push({ dayOfWeek: "Monday", label: "Monday", open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closedThisDay: false });
		    $scope.model.value.weekDays.push({ dayOfWeek: "Tuesday", label: "Tuesday", open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closedThisDay: false });
		    $scope.model.value.weekDays.push({ dayOfWeek: "Wednesday", label: "Wednesday", open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closedThisDay: false });
		    $scope.model.value.weekDays.push({ dayOfWeek: "Thursday", label: "Thursday", open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closedThisDay: false });
		    $scope.model.value.weekDays.push({ dayOfWeek: "Friday", label: "Friday", open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closedThisDay: false });
		    $scope.model.value.weekDays.push({ dayOfWeek: "Saturday", label: "Saturday", open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closedThisDay: false });
		    $scope.model.value.weekDays.push({ dayOfWeek: "Sunday", label: "Sunday", open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() }, closedThisDay: false });

			$scope.model.value.lastSundayInMonth.open = { view: 'datepicker', config: dateTimePickerConfig, value: new Date() };
			$scope.model.value.lastSundayInMonth.closed = { view: 'datepicker', config: dateTimePickerConfig, value: new Date() };

			$scope.model.value.firstSundayInMonth.open = { view: 'datepicker', config: dateTimePickerConfig, value: new Date() };
			$scope.model.value.firstSundayInMonth.closed = { view: 'datepicker', config: dateTimePickerConfig, value: new Date() };
		}

		$scope.createNewOpeningDay = function () {
			var openingDay = {
				text: "",
				selectedDate: {
					view: 'datepicker',
					config: {
						pickDate: true,
						pickTime: false,
						pick12HourFormat: false,
						format: "YYYY-MM-DD"
					},
					value: new Date()
				},
				open: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() },
				closed: { view: 'datepicker', config: dateTimePickerConfig, value: new Date() },
				closedThisDay: false
			};

			$scope.model.value.specificDays.push(openingDay);
		};

		$scope.deleteOpeningDay = function (day) {
			var index = $scope.model.value.specificDays.indexOf(day);
			$scope.model.value.specificDays.splice(index, 1);
		};
	});
}());