using System;

#if WINDOWS_PHONE
using System.Windows.Data;
#else
using Windows.UI.Xaml.Data;
#endif // WINDOWS_PHONE

namespace Charmed.Converters
{
	public sealed class InverseBoolConverter : IValueConverter
	{
#if WINDOWS_PHONE
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#else
		public object Convert(object value, Type targetType, object parameter, string language)
#endif // WINDOWS_PHONE
		{
			return Invert(value);
		}

#if WINDOWS_PHONE
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#else
		public object ConvertBack(object value, Type targetType, object parameter, string language)
#endif // WINDOWS_PHONE
		{
			return Invert(value);
		}

		private static object Invert(object value)
		{
			var result = false;
			if (value is bool)
			{
				return !(bool)value;
			}

			return result;
		}
	}
}
