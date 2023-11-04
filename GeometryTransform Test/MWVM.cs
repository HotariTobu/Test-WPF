using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingCircle_Test
{
    public class MWVM : SharedWPF.ViewModelBase
    {
        #region == X ==

        private double _X;
        public double X
        {
            get => _X;
            set
            {
                if (_X != value)
                {
                    _X = value;
                    RaisePropertyChanged(nameof(X));
                }
            }
        }

        #endregion
        #region == Y ==

        private double _Y;
        public double Y
        {
            get => _Y;
            set
            {
                if (_Y != value)
                {
                    _Y = value;
                    RaisePropertyChanged(nameof(Y));
                }
            }
        }

        #endregion
        #region == Angle ==

        private double _Angle;
        public double Angle
        {
            get => _Angle;
            set
            {
                if (_Angle != value)
                {
                    _Angle = value;
                    RaisePropertyChanged(nameof(Angle));
                }
            }
        }

        #endregion

        public MWVM()
        {
            X = 0;
            Y = 0;
            Angle = 0;
        }
    }
}
