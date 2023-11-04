#define FILE_WATCHER
#define MEDIA

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX.D3DCompiler;

using IOPath = System.IO.Path;

namespace WPFShaderEditor
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{

		public MainWindow()
		{
			InitializeComponent();
			sm = new ShaderManager();
			DataContext = sm;

			txtboxMediaFileName.Text = "sample.png";
			ButtonPlay_OnClick(null, null);

#if FILE_WATCHER
			WatchFXFile();
#endif
		}

		private ShaderManager sm;

#if FILE_WATCHER

		private FileSystemWatcher fw;

		private void WatchFXFile()
        {
			var dir = IOPath.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var fname = "Shader.fx";

			fw = new FileSystemWatcher();

			Action<string> fl = (path) =>
			{
				if (File.Exists(path))
				{
					for (int i = 0; i < 10; i++)
					{
						try
						{
							using (var sr = new StreamReader(path))
							{
								var s = sr.ReadToEnd();
								Dispatcher.BeginInvoke((Action)(() =>
								{
									sm.Source.Value = s;
								}),
									null);
								return;
							}
						}
						catch
						{
						}
						System.Threading.Thread.Sleep(100);
					}
				}
			};
			fl(IOPath.Combine(dir, fname));

			fw.Path = dir;

			fw.Changed += (s, e) =>
			{
				if (fname.Equals(e.Name))
				{
					fl(e.FullPath);
				}
			};

			fw.EnableRaisingEvents = true;

			Closed += (s, e) =>
			{
				fw.Dispose();
			};
		}

#endif

#if MEDIA
		private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
		{
			mediaElement.Position = TimeSpan.Zero;
			mediaElement.Play();
		}

		private void ButtonOpenFile_OnClick(object sender, EventArgs e)
		{
			var d = new Microsoft.Win32.OpenFileDialog();
			d.FileName = txtboxMediaFileName.Text;
			if (d.ShowDialog().GetValueOrDefault())
			{
				txtboxMediaFileName.Text = d.FileName;
			}
		}

		private void ButtonPlay_OnClick(object sender, EventArgs e)
		{
			try
			{
				mediaElement.Source = new Uri(txtboxMediaFileName.Text, UriKind.RelativeOrAbsolute);
				mediaElement.Play();
				mediaElement.Visibility = Visibility.Visible;
			}
			catch
			{
				mediaElement.Pause();
				mediaElement.Visibility = Visibility.Collapsed;
			}
		}

		private void ButtonReset_OnClick(object sender, EventArgs e)
		{
			mediaElement.Pause();
			mediaElement.Visibility = Visibility.Collapsed;
		}
#else
		private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e) { }

		private void ButtonOpenFile_OnClick(object sender, EventArgs e)
		{
			var d = new Microsoft.Win32.OpenFileDialog();
			d.FileName = txtboxMediaFileName.Text;
			if (d.ShowDialog().GetValueOrDefault())
			{
				txtboxMediaFileName.Text = d.FileName;
			}
		}

		private void ButtonPlay_OnClick(object sender, EventArgs e) { }

		private void ButtonReset_OnClick(object sender, EventArgs e) { }
#endif
	}
}
