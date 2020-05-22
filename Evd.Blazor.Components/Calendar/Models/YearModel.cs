using System.Collections.Generic;

namespace Evd.Blazor.Components.Calendar.Models
{
    public class YearModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public IEnumerable<WeekModel> Weeks { get; set; }
    }
}
