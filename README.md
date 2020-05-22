# BlazorComponents library
This library includes several components to use with Blazor.

Available components:

- EvdCalendar which is perfect for booking applications


To install the library don't forget to do the following steps:

- Add to your _Host.html file.
```html 
<link rel="stylesheet" href="_content/Evd.Blazor.Components/styles/calendar.css" />
``` 

- Add to your _Imports.razor file.
```csharp 
@using Evd.Blazor.Components.Calendar
@using Evd.Blazor.Components.Calendar.Models 
```

After these settings you can use the calendar like in our sample 'CalendarSample'.