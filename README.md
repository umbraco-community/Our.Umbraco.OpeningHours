# Our.Umbraco.OpeningHours
An Umbraco property editor for handling opening hours

### Installation

1. [**NuGet Package**][NuGetPackageUrl]  
Install this NuGet package in your Visual Studio project. Makes updating easy.

2. [**Umbraco package**][UmbracoPackageUrl]  
Install this Umbraco package via the developer section in Umbraco.

3. [**ZIP file**][GitHubReleaseUrl]  
Manually unzip and move files to the root directory of your website.

### Usage

To get the opening hours, you can simply use the `GetPropertyValue` method on the `IPublishedContent` holding the opening hours. Eg. as shown in this Razor partial:

```C#
@using Our.Umbraco.OpeningHours.Extensions
@using Our.Umbraco.OpeningHours.Model

@inherits UmbracoViewPage

@{

    OpeningHoursModel openingHours = Model.GetPropertyValue<OpeningHours>("openingHours");

}
```

Alternatively you can use a special extension called `GetOpeningHours`. This method will ensure that you always get an instance of `OpeningHoursModel` - even if the property doesn't exist or doesn't have a value. The code for this would look like:

```C#
@using Our.Umbraco.OpeningHours.Extensions
@using Our.Umbraco.OpeningHours.Model

@inherits UmbracoViewPage

@{

    OpeningHoursModel openingHours = Model.GetOpeningHours();

}
```

If using the **Opening Hours** property editor, you could list each weekday like in the example below.

Once you have an instance of `OpeningHours`, you can either iterate through the weekdays using the `openingHours.Weekdays` property, or get a specific weekday like `openingHours.Weekdays[DayOfWeek.Monday]`.

```C#
@using Our.Umbraco.OpeningHours.Extensions
@using Our.Umbraco.OpeningHours.Model
@using Our.Umbraco.OpeningHours.Model.Items

@inherits UmbracoViewPage

@{

    OpeningHoursModel openingHours = Model.GetOpeningHours();

    <div>
        We're currently <strong>@(openingHours.IsCurrentlyOpen ? "OPEN" : "CLOSED")</strong>
    </div>

    <hr />

    <table>
        @foreach (KeyValuePair<DayOfWeek, OpeningHoursWeekdayItem> pair in openingHours.Weekdays) {
            <tr>
                <td>
                    <strong>@pair.Key</strong>
                </td>
                <td>
                    @if (pair.Value.IsOpen) {
                        @(String.Join(", ", from item in pair.Value.Items select item.Opens + " - " + item.Closes))
                    } else {
                        <em>Closed!</em>
                    }
                </td>
            </tr>
        }
    </table>

}
```

The above example only represents weekdays - not specific dates. The property editor however also allows editors to specify custom dates - eg. if a store is closed for the holidays.

In the Razor example below, the next 10 days are shown whether the store is closed or the their opening hours if open:

```C#
@using Our.Umbraco.OpeningHours.Extensions
@using Our.Umbraco.OpeningHours.Model

@inherits UmbracoViewPage

@{

    OpeningHoursModel openingHours = Model.GetOpeningHours();
    
    <div>
        We're currently <strong>@(openingHours.IsCurrentlyOpen ? "OPEN" : "CLOSED")</strong>
    </div>

    <table class="openingHours">
        <thead>
            <tr>
                <th>Day</th>
                <th>Opening hours</th>
            </tr>
        </thead>
        <tbody>
            @for (DateTime dt = DateTime.Today; dt <= DateTime.Now.AddDays(7); dt = dt.AddDays(1)) {
                OpeningHoursDay day = openingHours.GetDay(dt);
                <tr>
                    <td>
                        <strong>@day.Label</strong>
                        <span style="color: #666; font-size: 11px;">(@day.Date.ToLongDateString())</span>
                    </td>
                    <td>
                        @if (day.IsOpen) {
                            @String.Join(", ", from item in day.Items select item.Opens.ToString("HH:mm") + " - " + item.Closes.ToString("HH:mm"))
                        } else {
                            <em>Closed!</em>
                        }
                    </td>
                </tr>
            }
        </tbody>
    </table>

}
```



[NuGetPackageUrl]: https://www.nuget.org/packages/Our.Umbraco.OpeningHours/1.0.0-beta1
[UmbracoPackageUrl]: https://our.umbraco.org/projects/backoffice-extensions/ourumbracoopeninghours/
[GitHubReleaseUrl]: https://github.com/bomortensen/Our.Umbraco.OpeningHours/releases/tag/v1.0.0-beta1
