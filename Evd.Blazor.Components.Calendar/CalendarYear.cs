using System.Collections.Generic;

namespace Evd.Blazor.Components.Calendar
{
    public class CalendarYear
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public IEnumerable<CalendarWeek> Weeks { get; set; }
    }
}
