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

If using the **Opening Hours** property editor, you could list each weekday like in the example below.

Once you have an instance of `OpeningHours`, you can either iterate through the weekdays using the `openingHours.Weekdays` property, or get a specific weekday like `openingHours.Weekdays[DayOfWeek.Monday]`.

```C#
@using Our.Umbraco.OpeningHours.Model
@inherits UmbracoViewPage
              
@{

    OpeningHours openingHours = Model.GetPropertyValue<OpeningHours>("openingHours");
    
    <div>
        We're currently <strong>@(openingHours.IsCurrentlyOpen ? "OPEN" : "CLOSED")</strong>
    </div>
    
    <hr />

    <table>
        @foreach (KeyValuePair<DayOfWeek, WeekdayOpeningHours> pair in openingHours.Weekdays) {
            <tr>
                <td>
                    <strong>@pair.Key</strong>
                </td>
                <td>
                    @if (pair.Value.IsOpen) {
                        <text>@pair.Value.Opens&ndash;@pair.Value.Closes</text>
                    } else {
                        <em>Closed</em>
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
@using Our.Umbraco.OpeningHours.Model
@inherits UmbracoViewPage
              
@{

    OpeningHours openingHours = Model.GetPropertyValue<OpeningHours>("openingHours");
    
    <div>
        We're currently <strong>@(openingHours.IsCurrentlyOpen ? "OPEN" : "CLOSED")</strong>
    </div>
    
    <hr />

    <table>
        @foreach (OpeningHoursDay day in openingHours.GetUpcomingDays(10)) {
            <tr>
                <th>@(day.HasLabel ? day.Label : day.WeekDayName):</th>
                @if (day.IsClosed) {
                    <td>Closed</td>
                } else {
                    <td>
                        @day.Opens.ToString("HH:mm")
                        &ndash;
                        @day.Closes.ToString("HH:mm")
                    </td>
                }
            </tr>
        }
    </table>

}
```



[NuGetPackageUrl]: https://www.nuget.org/packages/Our.Umbraco.OpeningHours/1.0.0-beta1
[UmbracoPackageUrl]: https://our.umbraco.org/projects/backoffice-extensions/ourumbracoopeninghours/
[GitHubReleaseUrl]: https://github.com/bomortensen/Our.Umbraco.OpeningHours/releases/tag/v1.0.0-beta1
