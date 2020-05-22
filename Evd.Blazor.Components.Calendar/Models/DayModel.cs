using System;

namespace Evd.Blazor.Components.Calendar.Models
{
    public class DayModel
    {
        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DayStatus Status { get; set; }
        public bool SelectionDisabled { get; set; }
    }
}
