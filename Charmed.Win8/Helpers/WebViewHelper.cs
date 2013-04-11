using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed.Helpers
{
	public sealed class WebViewHelper : DependencyObject
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
