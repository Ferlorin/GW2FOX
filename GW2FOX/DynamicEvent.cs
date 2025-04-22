using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GW2FOX
{
    // 👁️‍🗨️ Kann öffentlich bleiben, da es ein einfaches Datenmodell ist
    public class DynamicEventState
    {
        public string BossName { get; set; } = "";
        public TimeSpan Delay { get; set; }
        public string Category { get; set; } = "";
        public string Waypoint { get; set; } = "";
        public DateTime? StartTime { get; set; } // UTC
    }

    // ✅ Zugriff auf public gesetzt, damit DynamicEventStorage.Save/Load ihn verwenden kann
    public class DynamicEvent
    {
        public string BossName { get; }
        public TimeSpan Delay { get; }
        public string Category { get; }
        public string Waypoint { get; }
        public DateTime? StartTime { get; private set; }

        public DynamicEvent(string bossName, TimeSpan delay, string category, string waypoint)
        {
            BossName = bossName;
            Delay = delay;
            Category = category;
            Waypoint = waypoint;
        }

        public void Trigger()
        {
            StartTime = DateTime.UtcNow;
            Console.WriteLine($"[Trigger] {BossName} triggered at {StartTime.Value} (UTC)");
        }

        public void SetStartTime(DateTime utcStart)
        {
            StartTime = DateTime.SpecifyKind(utcStart, DateTimeKind.Utc);
            DebugTools.DebugTime($"{BossName} [SetStartTime]", StartTime.Value);
        }

        public bool IsRunning => StartTime.HasValue && DateTime.UtcNow < StartTime.Value + Delay;

        public BossEventRun ToBossEventRun()
        {
            if (!StartTime.HasValue)
                throw new InvalidOperationException("Event not triggered");

            var nextRunTime = StartTime.Value + Delay;

            return new BossEventRun(
                bossName: BossName,
                timing: Delay,
                category: Category,
                nextRunTime: nextRunTime,
                waypoint: Waypoint
            );
        }

        public DynamicEventState ToState()
        {
            return new DynamicEventState
            {
                BossName = BossName,
                Delay = Delay,
                Category = Category,
                Waypoint = Waypoint,
                StartTime = StartTime
            };
        }

        public static DynamicEvent FromState(DynamicEventState state)
        {
            var ev = new DynamicEvent(state.BossName, state.Delay, state.Category, state.Waypoint);
            if (state.StartTime.HasValue)
                ev.SetStartTime(state.StartTime.Value);
            return ev;
        }
    }

    public static class DynamicEventStorage
    {
        private const string FilePath = "dynamic_events.json";

        public static void Save(List<DynamicEvent> events)
        {
            var states = events.Select(e => e.ToState()).ToList();
            var json = JsonConvert.SerializeObject(states, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static List<DynamicEvent> Load()
        {
            if (!File.Exists(FilePath))
                return new List<DynamicEvent>();

            var json = File.ReadAllText(FilePath);
            var states = JsonConvert.DeserializeObject<List<DynamicEventState>>(json) ?? new();
            return states.Select(DynamicEvent.FromState).ToList();
        }
    }
}
