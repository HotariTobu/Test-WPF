using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingCircle_Test
{
    public class MWVM : SharedWPF.ViewModelBase
    {
        #region == LX ==

        private double _LX;
        public double LX
        {
            get => _LX;
            set
            {
                if (_LX != value)
                {
                    _LX = value;
                    RaisePropertyChanged(nameof(LX));
                }
            }
        }

        #endregion
        #region == LY ==

        private double _LY;
        public double LY
        {
            get => _LY;
            set
            {
                if (_LY != value)
                {
                    _LY = value;
                    RaisePropertyChanged(nameof(LY));
                }
            }
        }

        #endregion
        #region == LW ==

        private double _LW;
        public double LW
        {
            get => _LW;
            set
            {
                if (_LW != value)
                {
                    _LW = value;
                    RaisePropertyChanged(nameof(LW));
                }
            }
        }

        #endregion
        #region == LH ==

        private double _LH;
        public double LH
        {
            get => _LH;
            set
            {
                if (_LH != value)
                {
                    _LH = value;
                    RaisePropertyChanged(nameof(LH));
                }
            }
        }

        #endregion

        #region == L1X ==

        private double _L1X;
        public double L1X
        {
            get => _L1X;
            set
            {
                if (_L1X != value)
                {
                    _L1X = value;
                    RaisePropertyChanged(nameof(L1X));
                }
            }
        }

        #endregion
        #region == L1Y ==

        private double _L1Y;
        public double L1Y
        {
            get => _L1Y;
            set
            {
                if (_L1Y != value)
                {
                    _L1Y = value;
                    RaisePropertyChanged(nameof(L1Y));
                }
            }
        }

        #endregion
        #region == L2X ==

        private double _L2X;
        public double L2X
        {
            get => _L2X;
            set
            {
                if (_L2X != value)
                {
                    _L2X = value;
                    RaisePropertyChanged(nameof(L2X));
                }
            }
        }

        #endregion
        #region == L2Y ==

        private double _L2Y;
        public double L2Y
        {
            get => _L2Y;
            set
            {
                if (_L2Y != value)
                {
                    _L2Y = value;
                    RaisePropertyChanged(nameof(L2Y));
                }
            }
        }

        #endregion

        #region == RX ==

        private double _RX;
        public double RX
        {
            get => _RX;
            set
            {
                if (_RX != value)
                {
                    _RX = value;
                    RaisePropertyChanged(nameof(RX));
                }
            }
        }

        #endregion
        #region == RY ==

        private double _RY;
        public double RY
        {
            get => _RY;
            set
            {
                if (_RY != value)
                {
                    _RY = value;
                    RaisePropertyChanged(nameof(RY));
                }
            }
        }

        #endregion
        #region == RW ==

        private double _RW;
        public double RW
        {
            get => _RW;
            set
            {
                if (_RW != value)
                {
                    _RW = value;
                    RaisePropertyChanged(nameof(RW));
                }
            }
        }

        #endregion
        #region == RH ==

        private double _RH;
        public double RH
        {
            get => _RH;
            set
            {
                if (_RH != value)
                {
                    _RH = value;
                    RaisePropertyChanged(nameof(RH));
                }
            }
        }

        #endregion

        #region == R1X ==

        private double _R1X;
        public double R1X
        {
            get => _R1X;
            set
            {
                if (_R1X != value)
                {
                    _R1X = value;
                    RaisePropertyChanged(nameof(R1X));
                }
            }
        }

        #endregion
        #region == R1Y ==

        private double _R1Y;
        public double R1Y
        {
            get => _R1Y;
            set
            {
                if (_R1Y != value)
                {
                    _R1Y = value;
                    RaisePropertyChanged(nameof(R1Y));
                }
            }
        }

        #endregion
        #region == R2X ==

        private double _R2X;
        public double R2X
        {
            get => _R2X;
            set
            {
                if (_R2X != value)
                {
                    _R2X = value;
                    RaisePropertyChanged(nameof(R2X));
                }
            }
        }

        #endregion
        #region == R2Y ==

        private double _R2Y;
        public double R2Y
        {
            get => _R2Y;
            set
            {
                if (_R2Y != value)
                {
                    _R2Y = value;
                    RaisePropertyChanged(nameof(R2Y));
                }
            }
        }

        #endregion

        public MWVM()
        {
            LX = 0;
            LY = 0;
            LW = 0;
            LH = 0;

            L1X = 0;
            L1Y = 0;
            L2X = 0;
            L2Y = 0;

            RX = 0;
            RY = 0;
            RW = 0;
            RH = 0;

            R1X = 0;
            R1Y = 0;
            R2X = 0;
            R2Y = 0;
        }
    }
}
