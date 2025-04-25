using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GW2FOX
{
    public class BossListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string _countdown;
        public string Countdown
        {
            get => _countdown;
            set
            {
                if (_countdown != value)
                {
                    _countdown = value;
                    OnPropertyChanged(nameof(Countdown));
                }
            }
        }

        public string BossName { get; set; }
        public string Waypoint { get; set; }
        public string Category { get; set; }
        public string TimeRemainingFormatted { get; set; }
        public int SecondsRemaining { get; set; }
        public DateTime NextRunTime { get; set; }
        public bool IsPastEvent { get; set; }
        public bool IsDynamicEvent { get; set; }
        public bool IsConcurrentEvent { get; set; }
        public DateTime TimeToShow => NextRunTime;

        public void UpdateCountdown()
        {
            var timeLeft = NextRunTime - GlobalVariables.CURRENT_DATE_TIME;
            Countdown = timeLeft > TimeSpan.Zero
                ? timeLeft.ToString(@"hh\:mm\:ss")
                : "Runs";
        }
    }
}
