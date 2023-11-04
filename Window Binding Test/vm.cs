using System;
using System.Collections.Generic;
using System.Text;

namespace Window_Binding_Test
{
    class vm : SharedWPF.ViewModelBase
    {

        #region == l ==

        private double _l;
        public double l
        {
            get => _l;
            set
            {
                if (_l != value)
                {
                    _l = value;
                    RaisePropertyChanged(nameof(l));
                }
            }
        }

        #endregion

        #region == t ==

        private double _t;
        public double t
        {
            get => _t;
            set
            {
                if (_t != value)
                {
                    _t = value;
                    RaisePropertyChanged(nameof(t));
                }
            }
        }

        #endregion

        #region == w ==

        private double _w;
        public double w
        {
            get => _w;
            set
            {
                if (_w != value)
                {
                    _w = value;
                    RaisePropertyChanged(nameof(w));
                }
            }
        }

        #endregion

        #region == h ==

        private double _h;
        public double h
        {
            get => _h;
            set
            {
                if (_h != value)
                {
                    _h = value;
                    RaisePropertyChanged(nameof(h));
                }
            }
        }

        #endregion
    }
}
