using Microsoft.Phone.Controls;
using System.Windows;

namespace Charmed.Helpers
{
	/// <summary>
	/// Helper class to assist with making Html content bindable on a WebBrowser.
	/// </summary>
	public sealed class WebBrowserHelper
	{
		public static readonly DependencyProperty HtmlContentProperty =
			DependencyProperty.RegisterAttached(
				"HtmlContent",
				typeof(string),
				typeof(WebBrowserHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var webBrowser = sender as WebBrowser;
					if (webBrowser != null)
					{
						webBrowser.NavigateToString(args.NewValue.ToString());
					}
				})));

		public static void SetHtmlContent(
			WebBrowser attached,
			string htmlContent)
		{
			attached.SetValue(HtmlContentProperty, htmlContent);
		}

		public static string GetHtmlContent(WebBrowser attached)
		{
			return (string)attached.GetValue(HtmlContentProperty);
		}
	}
}
