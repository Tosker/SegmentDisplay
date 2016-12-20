using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SegmentDisplay
{
    public class SegmentViewModel : ObservableObject
    {
        //Timer and index for count down logic
        private DispatcherTimer _countTimer;
        private int _countIndex = 0;

        //Using RelayCommands for executing degment display  logic
        public RelayCommand ShowDigitCommand { get; private set; }
        public RelayCommand CountDownCommand { get; private set; }

        //The displayed number from segments
        private bool[] _number;
        public bool[] Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                OnPropertyChanged("Number");
            }
        }

        public SegmentViewModel()
        {
            ShowDigitCommand = new RelayCommand(ShowDigit, CanShowDigit);
            CountDownCommand = new RelayCommand(CountDown, CanShowDigit);
            _countTimer = new DispatcherTimer();
            _countTimer.Interval = new TimeSpan(0, 0, 1);
            _countTimer.Tick += count_tick;
        }

        //2D array for how the 7-point segement should be displayed
        private bool[][] _digits =
        {
            new bool[]{true, true, true, true, true, true, false}, // zero
            new bool[]{false, true, true, false, false, false, false}, // one
            new bool[]{true, true, false, true, true, false, true}, // two
            new bool[]{true, true, true, true, false, false, true}, // three
            new bool[]{false, true, true, false, false, true, true}, // four
            new bool[]{true, false, true, true, false, true, true}, // five
            new bool[]{true, false, true, true, true, true, true}, // six
            new bool[]{true, true, true, false, false, false, false}, // seven
            new bool[]{true, true, true, true, true, true, true}, // eight
            new bool[]{true, true, true, false, false, true, true}, // nine
        };

        //Display the number as a 7-point segment
        public void ShowDigit(object number)
        {
            int num = 0;
            int.TryParse((string)number, out num);
            Number = _digits[num];
        }

        //The current value can be displayed as the segment
        public bool CanShowDigit(object number)
        {
            int num = 0;
            if(int.TryParse((string)number, out num))
            {
                if (num >= 0 && num <= 9)
                {
                    return true;
                }
            }
            return false;
        }

        //Begin the count down from the supplied value
        public void CountDown(object number)
        {
            int num = 0;
            int.TryParse((string)number, out num);
            _countIndex = num;
            _countTimer.Start();
        }

        //Time has passed to continue count down
        private void count_tick(object sender, EventArgs e)
        {
            Number = _digits[_countIndex];

            if (_countIndex > 0)
                _countIndex--;
            else
                _countTimer.Stop();
        }
    }
}
