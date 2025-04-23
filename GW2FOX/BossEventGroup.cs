using GW2FOX;

public class BossEventGroup
{
    public string BossName { get; }

    private readonly List<BossEvent> _timings;
    private const int DaysExtraToCalculate = 1;

    public BossEventGroup(string bossName, IEnumerable<BossEvent> bossEvents)
    {
        BossName = bossName;
        _timings = bossEvents
            .Where(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.Timing)
            .ToList();
    }

    // Optional: Zugriff auf interne Liste (kann hilfreich sein)
    public List<BossEvent> Events => _timings;

    /// <summary>
    /// Liefert alle kommenden (und kürzlich vergangenen) Runs für diesen Boss.
    /// </summary>
    public IEnumerable<BossEventRun> GetNextRuns()
    {
        List<BossEventRun> result = new();

        for (int i = -1; i <= DaysExtraToCalculate; i++)
        {
            result.AddRange(_timings.Select(e => new BossEventRun(
                e.BossName,
                e.Timing,
                e.Category,
                GlobalVariables.CURRENT_DATE_TIME.Date.AddDays(i) + e.Timing,
                e.Waypoint
            )));
        }

        DateTime now = GlobalVariables.CURRENT_DATE_TIME;

        var filtered = result
            .Where(run =>
                    run.TimeToShow >= now - TimeSpan.FromMinutes(15) &&
    run.TimeToShow <= now + TimeSpan.FromHours(8))   // 8h Vorschau
            .OrderBy(run => run.TimeToShow)
            .ToList();

        // 🔍 Debug-Ausgabe aller relevanten Runs
        Console.WriteLine($"[GetNextRuns] {BossName}: {filtered.Count} Runs im sichtbaren Bereich");
        foreach (var run in filtered)
        {
            var inMin = (run.TimeToShow - now).TotalMinutes;
            Console.WriteLine($" - {run.BossName} @ {run.TimeToShow:HH:mm} ({inMin:F0} min)");
        }

        return filtered;
    }

    /// <summary>
    /// Gibt ALLE Events (unabhängig von Zeitpunkt) zurück – als Fallback oder Liste.
    /// </summary>
    public IEnumerable<BossEventRun> GetAllRuns()
    {
        List<BossEventRun> result = new();

        for (int i = -1; i <= DaysExtraToCalculate; i++)
        {
            result.AddRange(_timings.Select(e => new BossEventRun(
                e.BossName,
                e.Timing,
                e.Category,
                GlobalVariables.CURRENT_DATE_TIME.Date.AddDays(i) + e.Timing,
                e.Waypoint
            )));
        }

        return result.OrderBy(run => run.TimeToShow);
    }
}
