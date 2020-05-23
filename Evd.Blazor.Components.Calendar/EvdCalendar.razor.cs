using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Evd.Blazor.Components.Calendar
{
    public partial class EvdCalendar
    {
        [Parameter]
        public int StartYear { get; set; } = DateTime.Now.Year;

        [Parameter]
        public int StartMonth { get; set; } = DateTime.Now.Month;

        [Parameter]
        public int MinimumDaysToSelect { get; set; } = 5;

        [Parameter]
        public int MaximumMonthsToDisplay { get; set; } = 12;

        [Parameter]
        public IEnumerable<DateRange> OccupiedDateRanges { get; set; }

        [Parameter]
        public EventCallback<DateRange> OnDateRangeSelected { get; set; }

        [Parameter]
        public string Culture { get; set; } = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        private DateTime? startDate { get; set; }
        private DateTime? endDate { get; set; }
        private IEnumerable<CalendarYear> calendarData;
        private CultureInfo currentCulture;
        private DateTime currentDate;

        private IEnumerable<DayOfWeek> DayOfWeekColumns =>
                                                Enum
                                                    .GetValues(typeof(DayOfWeek))
                                                    .OfType<DayOfWeek>()
                                                    .SkipWhile(d => d != currentCulture.DateTimeFormat.FirstDayOfWeek)
                                                    .Concat(Enum
                                                            .GetValues(typeof(DayOfWeek))
                                                            .OfType<DayOfWeek>()
                                                            .TakeWhile(d => d != currentCulture.DateTimeFormat.FirstDayOfWeek)
                                                    );

        protected override void OnInitialized()
        {
            if (currentCulture == null)
            {
                currentCulture = new CultureInfo(Culture);
                currentDate = new DateTime(StartYear, StartMonth, 1);
                calendarData = GetCalendarData();
            }
        }

        private IEnumerable<CalendarYear> GetCalendarData()
        {
            var calendar = Enumerable
                .Range(0, (int)currentDate.AddYears(1).Subtract(currentDate).TotalDays)
                .Select(d => currentDate.AddDays(d))
                .Select(d => new { Date = d, Weekday = currentCulture.Calendar.GetDayOfWeek(d), WeekNr = currentCulture.Calendar.GetWeekOfYear(d, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek) })
                .GroupBy(d => new { d.Date.Year, d.Date.Month, d.WeekNr })
                .GroupBy(d => new { d.Key.Year, d.Key.Month })
                .Take(MaximumMonthsToDisplay)
                .Select(y => new CalendarYear
                {
                    Month = y.Key.Month,
                    Year = y.Key.Year,
                    Weeks = y.Select(w => new CalendarWeek
                    {
                        Weeknumber = w.Key.WeekNr,
                        Days = w.Select(d => new CalendarDay
                        {
                            Date = d.Date,
                            DayOfWeek = d.Weekday,
                            Status = OccupiedDateRanges != null && OccupiedDateRanges.Any(o => d.Date >= o.Start && d.Date <= o.End) ? DayStatus.Occupied : DayStatus.Free
                        })
                    })
                });

            return calendar;
        }

        private DateTime? GetFirstOccupiedDateAfterStartDate()
        {
            if (OccupiedDateRanges == null || !startDate.HasValue)
                return null;

            return OccupiedDateRanges.OrderBy(o => o.Start).FirstOrDefault(o => startDate < o.Start)?.Start;
        }

        private void SelectStartDate(DateTime? date)
        {
            startDate = date;
        }

        private async Task SelectEndDate(DateTime? date)
        {
            endDate = date;
            await DateRangeSelected();
        }

        private void ResetStartAndEndDate()
        {
            startDate = null;
            endDate = null;
        }

        private async Task DateRangeSelected()
        {
            if (startDate < endDate)
            {
                await OnDateRangeSelected.InvokeAsync(new DateRange(startDate.Value, endDate.Value));
            }
        }

    }
}
