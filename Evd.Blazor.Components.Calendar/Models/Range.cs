namespace Evd.Blazor.Components.Calendar.Models
{
    public class Range<T1,T2>
    {
        public T1 Start { get; set; }
        public T2 End { get; set; }

        public Range() { }

        public Range(T1 startValue, T2 endValue)
        {
            Start = startValue;
            End = endValue;
        }
    }
}
