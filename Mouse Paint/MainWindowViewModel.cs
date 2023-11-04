using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace Mouse_Paint
{
    class MainWindowViewModel : SharedWPF.ViewModelBase
    {
        #region == Attributes ==

        public DrawingAttributes Attributes
        {
            get
            {
                DrawingAttributes attributes = new DrawingAttributes();
                attributes.Color = Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
                attributes.FitToCurve = FitToCurve;
                attributes.IgnorePressure = IgnorePressure;
                attributes.IsHighlighter = IsHighlighter;
                attributes.StylusTip = Tip;
                attributes.Width = Width;
                attributes.Height = Height;
                return attributes;
            }
        }

        #endregion

        #region == A ==

        private double _A;
        public double A
        {
            get => _A;
            set
            {
                if (_A != value)
                {
                    _A = value;
                    RaisePropertyChanged(nameof(A));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion
        #region == R ==

        private double _R;
        public double R
        {
            get => _R;
            set
            {
                if (_R != value)
                {
                    _R = value;
                    RaisePropertyChanged(nameof(R));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion
        #region == G ==

        private double _G;
        public double G
        {
            get => _G;
            set
            {
                if (_G != value)
                {
                    _G = value;
                    RaisePropertyChanged(nameof(G));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion
        #region == B ==

        private double _B;
        public double B
        {
            get => _B;
            set
            {
                if (_B != value)
                {
                    _B = value;
                    RaisePropertyChanged(nameof(B));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion

        #region == FitToCurve ==

        private bool _FitToCurve;
        public bool FitToCurve
        {
            get => _FitToCurve;
            set
            {
                if (_FitToCurve != value)
                {
                    _FitToCurve = value;
                    RaisePropertyChanged(nameof(FitToCurve));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion
        #region == IgnorePressure ==

        private bool _IgnorePressure;
        public bool IgnorePressure
        {
            get => _IgnorePressure;
            set
            {
                if (_IgnorePressure != value)
                {
                    _IgnorePressure = value;
                    RaisePropertyChanged(nameof(IgnorePressure));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion
        #region == IsHighlighter ==

        private bool _IsHighlighter;
        public bool IsHighlighter
        {
            get => _IsHighlighter;
            set
            {
                if (_IsHighlighter != value)
                {
                    _IsHighlighter = value;
                    RaisePropertyChanged(nameof(IsHighlighter));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion
        #region == Tip ==

        private StylusTip _Tip;
        public StylusTip Tip
        {
            get => _Tip;
            set
            {
                if (_Tip != value)
                {
                    _Tip = value;
                    RaisePropertyChanged(nameof(Tip));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion

        #region == Width ==

        private double _Width;
        public double Width
        {
            get => _Width;
            set
            {
                if (_Width != value)
                {
                    _Width = value;
                    RaisePropertyChanged(nameof(Width));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion
        #region == Height ==

        private double _Height;
        public double Height
        {
            get => _Height;
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    RaisePropertyChanged(nameof(Height));
                    RaisePropertyChanged(nameof(Attributes));
                }
            }
        }

        #endregion

        #region == EditMode ==

        private InkCanvasEditingMode _EditMode;
        public InkCanvasEditingMode EditMode
        {
            get => _EditMode;
            set
            {
                if (_EditMode != value)
                {
                    _EditMode = value;
                    RaisePropertyChanged(nameof(EditMode));
                }
            }
        }

        #endregion

        #region == IsFloat ==

        private bool _IsFloat;
        public bool IsFloat
        {
            get => _IsFloat;
            set
            {
                if (_IsFloat != value)
                {
                    _IsFloat = value;
                    RaisePropertyChanged(nameof(IsFloat));
                }
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            A = 255;

            Width = 3;
            Height = 3;

            EditMode = InkCanvasEditingMode.Ink;
        }
    }
}
