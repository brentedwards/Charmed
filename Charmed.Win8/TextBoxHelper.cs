using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed
{
	public class TextBoxHelper
	{
		public static readonly DependencyProperty BindableTextProperty =
			DependencyProperty.RegisterAttached(
				"BindableText",
				typeof(string),
				typeof(TextBoxHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var textBox = sender as TextBox;
					if (textBox != null)
					{
						textBox.Text = (string)args.NewValue;
						if (textBox.Tag == null)
						{
							textBox.Tag = new TextBoxHelper(textBox);
						}
					}
				})));

		public TextBoxHelper(TextBox textBox)
		{
			textBox.TextChanged += (o, a) =>
			{
				textBox.SetValue(BindableTextProperty, textBox.Text);
			};
		}

		public static void SetBindableText(TextBox element, string value)
		{
			element.SetValue(BindableTextProperty, value);
		}
		public static string GetBindableText(TextBox element)
		{
			return (string)element.GetValue(BindableTextProperty);
		}
	}
}
