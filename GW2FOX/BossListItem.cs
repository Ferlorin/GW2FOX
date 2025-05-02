using System;
using System.ComponentModel;

namespace GW2FOX
{
    public class BossListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Benachrichtigt das UI, wenn sich eine Eigenschaft ändert
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
        public string Waypoint { get; set; }
        public string Category { get; set; }
        public string TimeRemainingFormatted { get; set; }
        public int SecondsRemaining { get; set; }
        public DateTime NextRunTime { get; set; }
        public bool IsPastEvent { get; set; }
        public bool IsDynamicEvent { get; set; }
        public bool IsConcurrentEvent { get; set; }
        public DateTime TimeToShow => NextRunTime;

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
                    Console.WriteLine($"[DEBUG] SET ChestOpened for {BossName} = {value}");
                    OnPropertyChanged(nameof(ChestOpened));
                    OnPropertyChanged(nameof(ChestImagePath));
                }
            }
        }


        public void LoadChestState()
        {
            var value = BossTimings.IsChestOpened(BossName);
            Console.WriteLine($"[DEBUG] LOADED {BossName} chestOpened = {value}");

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



        public void UpdateCountdown()
        {
            var timeLeft = NextRunTime - GlobalVariables.CURRENT_DATE_TIME;
            Countdown = timeLeft > TimeSpan.Zero
                ? timeLeft.ToString(@"hh\:mm\:ss")
                : "Runs";
        }
    }
}
