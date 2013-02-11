using Edge.Core.Services;
using System;
using System.Windows.Data;

namespace Edge.Applications.PM.SchedulerControl.Converters
{
	public class Outcome2ImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var outcome = (ServiceOutcome)value;

			var iconPath = outcome == ServiceOutcome.Failure || outcome == ServiceOutcome.Success
				           ? String.Format("{0}\\Icons\\{1}.ico",Environment.CurrentDirectory, outcome)
				           : null;

			return iconPath;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
