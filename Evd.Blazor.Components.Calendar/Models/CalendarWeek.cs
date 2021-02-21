using System.Collections.Generic;

namespace Evd.Blazor.Components.Calendar
{
    public class CalendarWeek
    {
        public int Weeknumber { get; set; }
        public IEnumerable<CalendarDay> Days { get; set; }
    }
}