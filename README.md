## BlazorComponents library
This library includes several components to use with Blazor.

Available components:

- EvdCalendar which is perfect for booking applications
```csharp
/// For nuget installation use
Install-Package Evd.Blazor.Components.Calendar
```


To install the library don't forget to do the following steps:

- Add to your _Host.html file.
```html 
<link rel="stylesheet" href="_content/Evd.Blazor.Components.Calendar/styles.css" />
``` 

- Add to your _Imports.razor file.
```csharp 
@using Evd.Blazor.Components.Calendar
```

After these settings you can use the calendar like in our sample 'CalendarSample'.

![](https://github.com/edwinvandriel/BlazorComponents/blob/master/Images/EvdCalendar1.jpg)

![](https://github.com/edwinvandriel/BlazorComponents/blob/master/Images/EvdCalendar2.jpg)

![](https://github.com/edwinvandriel/BlazorComponents/blob/master/Images/EvdCalendar3.jpg)
