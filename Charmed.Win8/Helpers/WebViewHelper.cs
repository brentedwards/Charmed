using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed.Helpers
{
	/// <summary>
	/// Helper class to assist with making Html content bindable on a WebView.
	/// </summary>
	public sealed class WebViewHelper
	{
		public static readonly DependencyProperty HtmlContentProperty =
			DependencyProperty.RegisterAttached(
				"HtmlContent",
				typeof(string),
				typeof(WebViewHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var webView = sender as WebView;
					if (webView != null)
					{
						webView.NavigateToString(args.NewValue.ToString());
					}
				})));

		public static void SetHtmlContent(
			DependencyObject attached,
			string htmlContent)
		{
			attached.SetValue(HtmlContentProperty, htmlContent);
		}

		public static string GetHtmlContent(DependencyObject attached)
		{
			return (string)attached.GetValue(HtmlContentProperty);
		}
	}
}
