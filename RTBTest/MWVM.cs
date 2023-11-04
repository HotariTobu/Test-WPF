using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RTBTest
{
    internal class MWVM: SharedWPF.ViewModelBase
    {
        #region == StartIndex ==

        private int _StartIndex;
        public int StartIndex
        {
            get => _StartIndex;
            set
            {
                if (_StartIndex != value)
                {
                    _StartIndex = value;
                    RaisePropertyChanged(nameof(StartIndex));
                }
            }
        }

        #endregion
        #region == Length ==

        private int _Length;
        public int Length
        {
            get => _Length;
            set
            {
                if (_Length != value)
                {
                    _Length = value;
                    RaisePropertyChanged(nameof(Length));
                }
            }
        }

        #endregion

        #region == InsertionIndex ==

        private int _InsertionIndex;
        public int InsertionIndex
        {
            get => _InsertionIndex;
            set
            {
                if (_InsertionIndex != value)
                {
                    _InsertionIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region == NextContentIndex ==

        private int _NextContentIndex;
        public int NextContentIndex
        {
            get => _NextContentIndex;
            set
            {
                if (_NextContentIndex != value)
                {
                    _NextContentIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region == NextInsertionIndex ==

        private int _NextInsertionIndex;
        public int NextInsertionIndex
        {
            get => _NextInsertionIndex;
            set
            {
                if (_NextInsertionIndex != value)
                {
                    _NextInsertionIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region == BackwardRect ==

        private Rect _BackwardRect;
        public Rect BackwardRect
        {
            get => _BackwardRect;
            set
            {
                if (_BackwardRect != value)
                {
                    _BackwardRect = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region == ForwardRect ==

        private Rect _ForwardRect;
        public Rect ForwardRect
        {
            get => _ForwardRect;
            set
            {
                if (_ForwardRect != value)
                {
                    _ForwardRect = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        public MWVM()
        {
            StartIndex = 0;
            Length = 0;
        }
    }
}
