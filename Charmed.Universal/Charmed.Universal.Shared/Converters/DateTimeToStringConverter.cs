﻿using System;

#if WINDOWS_PHONE
using System.Windows.Data;
#else
using Windows.UI.Xaml.Data;
#endif // WINDOWS_PHONE

namespace Charmed.Converters
{
	/// <summary>
	/// Converts a DateTime to a string value.
	/// </summary>
	public sealed class DateTimeToStringConverter : IValueConverter
	{
		public string Format { get; set; }
		public bool IsDateTimeNullable { get; set; }

#if WINDOWS_PHONE
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#else
		public object Convert(object value, Type targetType, object parameter, string language)
#endif // WINDOWS_PHONE
		{
			var result = string.Empty;
			if (value is DateTime)
			{
				result = ((DateTime)value).ToString(this.Format ?? "d");
			}

			return result;
		}

#if WINDOWS_PHONE
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#else
		public object ConvertBack(object value, Type targetType, object parameter, string language)
#endif // WINDOWS_PHONE
		{
			DateTime date;
			if (value != null && DateTime.TryParse(value.ToString(), out date))
			{
				return date;
			}
			else
			{
				if (this.IsDateTimeNullable)
				{
					return null;
				}
				else
				{
					return new DateTime();
				}
			}
		}
	}
}
