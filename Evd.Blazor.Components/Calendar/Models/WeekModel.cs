using System.Collections.Generic;

namespace Evd.Blazor.Components.Calendar.Models
{
    public class WeekModel
    {
        public int Weeknumber { get; set; }
        public IEnumerable<DayModel> Days { get; set; }
    }
}