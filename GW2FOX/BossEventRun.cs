using System;
using System.ComponentModel;

namespace GW2FOX
{
    public class BossEventRun : BossEvent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime NextRunTime { get; set; }

        public BossEventRun(string bossName, TimeSpan timing, string category, DateTime nextRunTime, string waypoint = "", string level = "")
    : base(bossName, timing, category, waypoint, level)
        {
            NextRunTime = nextRunTime;
        }


        public DateTime TimeToShow => NextRunTime;

        public bool IsPreviousBoss => NextRunTime < GlobalVariables.CURRENT_DATE_TIME;

        public TimeSpan TimeRemaining =>
            IsPreviousBoss
                ? GlobalVariables.CURRENT_DATE_TIME.AddMinutes(15) - TimeToShow
                : TimeToShow - GlobalVariables.CURRENT_DATE_TIME;

        public string TimeRemainingFormatted =>
            $"{(int)TimeRemaining.TotalHours:D2}:{TimeRemaining.Minutes:D2}:{TimeRemaining.Seconds:D2}";

        public bool IsPastEvent
        {
            get => _isPastEvent;
            set
            {
                if (_isPastEvent != value)
                {
                    _isPastEvent = value;
                    OnPropertyChanged(nameof(IsPastEvent));
                }
            }
        }
        private bool _isPastEvent;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void TriggerTimeRemainingChanged() =>
            OnPropertyChanged(nameof(TimeRemainingFormatted));
    }
}
