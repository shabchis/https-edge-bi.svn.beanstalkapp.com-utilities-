using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Edge.Applications.PM.SchedulerControl.ViewModels;

namespace Edge.Applications.PM.SchedulerControl.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

		private void MainView_OnClosing(object sender, CancelEventArgs e)
		{
			var vm = DataContext as MainViewModel;
			if (vm != null)
			{
				vm.Dispose();
			}
		}
    }
}
