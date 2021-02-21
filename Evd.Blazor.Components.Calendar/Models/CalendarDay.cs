using System;

namespace Evd.Blazor.Components.Calendar
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DayStatus Status { get; set; }
        public bool SelectionDisabled { get; set; }
    }
}
