using System;
using static GW2FOX.BossTimings;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

namespace GW2FOX
{
    internal class DynamicEvent
    {
        public string BossName { get; }
        public TimeSpan Delay { get; }
        public string Category { get; }
        public string Waypoint { get; }

        // Persisted start time for the event
        public DateTime? StartTime { get; private set; }

        public DynamicEvent(string bossName, TimeSpan delay, string category, string waypoint)
        {
            BossName = bossName;
            Delay = delay;
            Category = category;
            Waypoint = waypoint;
        }

        /// <summary>
        /// Trigger the event and set the start time to now.
        /// </summary>
        public void Trigger()
        {
            StartTime = DateTime.UtcNow;
            Console.WriteLine($"[Trigger] {BossName} triggered at {StartTime.Value} (UTC)");
        }

        /// <summary>
        /// Manually set the start time (for loading persisted state).
        /// </summary>
        public void SetStartTime(DateTime utcStart)
        {
            StartTime = DateTime.SpecifyKind(utcStart, DateTimeKind.Utc);
            DebugTools.DebugTime($"{BossName} [SetStartTime]", StartTime.Value);
        }


        /// <summary>
        /// True if the event has been triggered and not yet expired.
        /// </summary>
        public bool IsRunning => StartTime.HasValue
                                 && DateTime.UtcNow < StartTime.Value + Delay;

        /// <summary>
        /// Convert to a BossEventRun for display in overlay.
        /// </summary>
        public BossEventRun ToBossEventRun()
        {
            if (!StartTime.HasValue)
                throw new InvalidOperationException("Event not triggered");

            // FINAL: Annahme, StartTime ist bereits lokal, Delay neutral
            DateTime nextRunTime = StartTime.Value + Delay;

            // KEINE zusätzliche Konvertierung – das ist der Fehler!
            return new BossEventRun(
                bossName: BossName,
                timing: Delay,
                category: Category,
                nextRunTime: nextRunTime,
                waypoint: Waypoint
            );
        }



    }
}