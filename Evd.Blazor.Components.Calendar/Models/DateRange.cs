using System;

namespace Evd.Blazor.Components.Calendar
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateRange(DateTime startDate, DateTime endDate) 
        {
            Start = startDate;
            End = endDate;
        }
    }
}
