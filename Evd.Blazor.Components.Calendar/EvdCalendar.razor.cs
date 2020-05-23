using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Evd.Blazor.Components.Calendar
{
    /// <summary>
    /// This calendar component is ideal for creating booking applications.
    /// </summary>
    public partial class EvdCalendar
    {
        /// <summary>
        /// The year to start the calendar in. Default is DateTime.Now.
        /// </summary>
        [Parameter]
        public int StartYear { get; set; } = DateTime.Now.Year;

        /// <summary>
        /// The month to start the calendar in. Default is DateTime.Now.
        /// </summary>
        [Parameter]
        public int StartMonth { get; set; } = DateTime.Now.Month;

        /// <summary>
        /// Minimum count of contiguous days to select. Default is 5 days.
        /// </summary>
        [Parameter]
        public int MinimumDaysToSelect { get; set; } = 5;

        /// <summary>
        /// The maximum count of months to show. Default is 12;
        /// </summary>
        [Parameter]
        public int MaximumMonthsToDisplay { get; set; } = 12;

        /// <summary>
        /// Input a collection of DateRange to display as occupied ranges.
        /// </summary>
        [Parameter]
        public IEnumerable<DateRange> OccupiedDateRanges { get; set; }

        /// <summary>
        /// Event received when Start- and End- date are selected.
        /// </summary>
        [Parameter]
        public EventCallback<DateRange> OnDateRangeSelected { get; set; }

        /// <summary>
        /// The two letter ISO language name. Default is from the machine that hosts the blazor application.
        /// Possible values are nl, en, nl-NL, en-US. All your host system cultures are supported.
        /// </summary>
        [Parameter]
        public string Culture { get; set; } = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        private DateTime? startDate { get; set; }
        private DateTime? endDate { get; set; }
        private IEnumerable<CalendarYear> calendarData;
        private CultureInfo currentCulture;
        private DateTime currentDate;

        /// <summary>
        /// Get all the weekdays in the right order for the calendar.
        /// </summary>
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

        /// <summary>
        /// Initialize the blazor application and set some things up.
        /// </summary>
        protected override void OnInitialized()
        {
            if (currentCulture == null)
            {
                currentCulture = new CultureInfo(Culture);
                currentDate = new DateTime(StartYear, StartMonth, 1);
                calendarData = GetCalendarData();
            }
        }

        /// <summary>
        /// Get all the data to build the calendar.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get the first occupied date after the selected start. To determine if we can select the MinimumDaysToSelect.
        /// </summary>
        /// <returns>DateTime</returns>
        private DateTime? GetFirstOccupiedDateAfterStartDate()
        {
            if (OccupiedDateRanges == null || !startDate.HasValue)
                return null;

            return OccupiedDateRanges.OrderBy(o => o.Start).FirstOrDefault(o => startDate < o.Start)?.Start;
        }

        /// <summary>
        /// Set the selected start date.
        /// </summary>
        /// <param name="date">Selected DateTime</param>
        private void SelectStartDate(DateTime? date)
        {
            startDate = date;
        }

        /// <summary>
        /// Set the selected end date.
        /// </summary>
        /// <param name="date">Selected DateTime</param>
        private async Task SelectEndDate(DateTime? date)
        {
            endDate = date;
            await DateRangeSelected();
        }

        /// <summary>
        /// Reset the start- and end- date to clear selection area.
        /// </summary>
        private void ResetStartAndEndDate()
        {
            startDate = null;
            endDate = null;
        }

        /// <summary>
        /// Fire the event if the start date is smaller than the end date.
        /// </summary>
        private async Task DateRangeSelected()
        {
            if (startDate < endDate)
            {
                await OnDateRangeSelected.InvokeAsync(new DateRange(startDate.Value, endDate.Value));
            }
        }

    }
}
