using System;

namespace GW2FOX
{
    public static class DebugTools
    {
        public static void DebugTime(string label, DateTime time)
        {
            Console.WriteLine($"[{label}] Raw: {time} | Kind: {time.Kind}");

            if (time.Kind == DateTimeKind.Utc)
            {
                var local = TimeZoneInfo.ConvertTimeFromUtc(time, GlobalVariables.TIMEZONE_TO_USE);
                Console.WriteLine($" → Interpretiert als LOCAL: {local} | DST: {GlobalVariables.TIMEZONE_TO_USE.IsDaylightSavingTime(local)}");
            }
            else if (time.Kind == DateTimeKind.Local)
            {
                var utc = time.ToUniversalTime();
                Console.WriteLine($" → Interpretiert als UTC: {utc}");
            }
            else
            {
                Console.WriteLine(" → WARNUNG: DateTimeKind.Unspecified – mögliche Fehlerquelle!");
            }

            Console.WriteLine($" → System.Now: {DateTime.Now} | UtcNow: {DateTime.UtcNow}");
            Console.WriteLine();
        }
    }
}
