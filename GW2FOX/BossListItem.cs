using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace GW2FOX
{
    public class BossListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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
        public string ChestImagePath => ChestOpened ? "/Resources/OpenChest.png" : "/Resources/Black_Lion_Chest.png";
        public string BossName { get; set; }
        public string Level { get; set; } = "";
        public string Waypoint { get; set; }
        public string Category { get; set; }
        private string _timeRemainingFormatted = "";
        public string TimeRemainingFormatted
        {
            get => _timeRemainingFormatted;
            set
            {
                if (_timeRemainingFormatted != value)
                {
                    _timeRemainingFormatted = value;
                    OnPropertyChanged(nameof(TimeRemainingFormatted));
                }
            }
        }
        private int _secondsRemaining;
        public int SecondsRemaining
        {
            get => _secondsRemaining;
            set
            {
                if (_secondsRemaining != value)
                {
                    _secondsRemaining = value;
                    OnPropertyChanged(nameof(SecondsRemaining));
                }
            }
        }
        public DateTime NextRunTime { get; set; }
        private bool _isPastEvent;
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
        public bool IsDynamicEvent { get; set; }
        private bool _isConcurrentEvent;
        public bool IsConcurrentEvent
        {
            get => _isConcurrentEvent;
            set
            {
                if (_isConcurrentEvent != value)
                {
                    _isConcurrentEvent = value;
                    OnPropertyChanged(nameof(IsConcurrentEvent));
                }
            }
        }
        public DateTime TimeToShow => NextRunTime;
        public List<LootHelper.LootResult> LootItems { get; set; } = new();


        // Eine Definition für ChestOpened
        private bool _chestOpened;
        public bool ChestOpened
        {
            get => _chestOpened;
            set
            {
                if (_chestOpened != value)
                {
                    _chestOpened = value;
                    OnPropertyChanged(nameof(ChestOpened));
                    OnPropertyChanged(nameof(ChestImagePath));
                }
            }
        }


        public void LoadChestState()
        {
            var value = BossTimings.IsChestOpened(BossName);

            if (ChestOpened != value)
            {
                ChestOpened = value;
            }
            else
            {
                OnPropertyChanged(nameof(ChestOpened));
                OnPropertyChanged(nameof(ChestImagePath));
            }
        }

        public void TriggerIconUpdate()
        {
            OnPropertyChanged(nameof(ChestImagePath));
        }

        public void UpdateCountdown()
        {
            var timeLeft = NextRunTime - GlobalVariables.CURRENT_DATE_TIME;
            Countdown = timeLeft > TimeSpan.Zero
                ? timeLeft.ToString(@"hh\:mm\:ss")
                : "Runs";
        }

        public void UpdateTimeProperties(DateTime now)
        {
            var remaining = NextRunTime - now;
            IsPastEvent = remaining.TotalSeconds < 0;

            var abs = remaining.Duration(); // absoluter Wert (für Formatierung)
            SecondsRemaining = (int)(IsPastEvent ? -abs.TotalSeconds : abs.TotalSeconds);

            TimeRemainingFormatted = IsPastEvent
                ? $"-{(int)abs.TotalHours:D2}:{abs.Minutes:D2}:{abs.Seconds:D2}"
                : $"{(int)abs.TotalHours:D2}:{abs.Minutes:D2}:{abs.Seconds:D2}";
        }

    }
}
