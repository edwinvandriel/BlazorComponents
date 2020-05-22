using System;

namespace Evd.Blazor.Components.Calendar.Models
{
    public class DateRange : Range<DateTime, DateTime>
    {
        public DateRange(DateTime startDate, DateTime endDate) : base(startDate, endDate) { }
    }
}
