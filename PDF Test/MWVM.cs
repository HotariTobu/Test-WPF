using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PDF_Test
{
    internal class MWVM: ViewModelBase
    {
        #region == Path ==

        private string _Path = "sample.pdf";
        public string Path
        {
            get => _Path;
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region == PageSources ==

        private ObservableCollection<ImageSource> _PageSources = new ObservableCollection<ImageSource>();
        public ObservableCollection<ImageSource> PageSources
        {
            get => _PageSources;
            set
            {
                _PageSources = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}
