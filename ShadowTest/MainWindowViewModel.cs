using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowTest
{
    public class MainWindowViewModel: SharedWPF.ViewModelBase
    {
        #region == BlurRadius ==

        private double _BlurRadius;
        public double BlurRadius
        {
            get => _BlurRadius;
            set
            {
                if (_BlurRadius != value)
                {
                    _BlurRadius = value;
                    RaisePropertyChanged(nameof(BlurRadius));
                }
            }
        }

        #endregion

        #region == ShadowDepth ==

        private double _ShadowDepth;
        public double ShadowDepth
        {
            get => _ShadowDepth;
            set
            {
                if (_ShadowDepth != value)
                {
                    _ShadowDepth = value;
                    RaisePropertyChanged(nameof(ShadowDepth));
                }
            }
        }

        #endregion

        #region == Direction ==

        private double _Direction;
        public double Direction
        {
            get => _Direction;
            set
            {
                if (_Direction != value)
                {
                    _Direction = value;
                    RaisePropertyChanged(nameof(Direction));
                }
            }
        }

        #endregion

        #region == Opacity ==

        private double _Opacity = 1;
        public double Opacity
        {
            get => _Opacity;
            set
            {
                if (_Opacity != value)
                {
                    _Opacity = value;
                    RaisePropertyChanged(nameof(Opacity));
                }
            }
        }

        #endregion

        #region == Radius ==

        private double _Radius;
        public double Radius
        {
            get => _Radius;
            set
            {
                if (_Radius != value)
                {
                    _Radius = value;
                    RaisePropertyChanged(nameof(Radius));
                }
            }
        }

        #endregion
    }
}
