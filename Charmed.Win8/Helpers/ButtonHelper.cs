using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed.Helpers
{
	public sealed class ButtonHelper
	{
		public static readonly DependencyProperty MethodNameProperty =
			DependencyProperty.RegisterAttached(
				"MethodName",
				typeof(string),
				typeof(ButtonHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var button = sender as Button;
					if (button != null)
					{
						if (!(button.Tag is ButtonHelper))
						{
							button.Tag = new ButtonHelper(button);
						}
					}
				})));

		public static readonly DependencyProperty ParameterProperty =
			DependencyProperty.RegisterAttached(
				"Parameter",
				typeof(object),
				typeof(ButtonHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var button = sender as Button;
					if (button != null)
					{
						if (!(button.Tag is ButtonHelper))
						{
							button.Tag = new ButtonHelper(button);
						}
					}
				})));

		public ButtonHelper(Button button)
		{
			button.Click += (o, a) =>
			{
				var methodName = GetMethodName(button);
				var method = button.DataContext.GetType().GetTypeInfo().GetDeclaredMethod(methodName);
				var parms = method.GetParameters();

				object[] parameters = null;

				if (parms != null && parms.Length > 0)
				{
					parameters = new object[parms.Length];
					if (parms[0].ParameterType.GetTypeInfo().IsAssignableFrom(o.GetType().GetTypeInfo()))
					{
						// The first parameter must be the clicked button.
						parameters[0] = o;
					}
					else
					{
						// The first parameter must be the ClickParameter
						parameters[0] = GetParameter(button);
					}

					if (parms.Length == 2)
					{
						// The second parameter must be the ClickParameter
						parameters[1] = GetParameter(button);
					}
				}

				method.Invoke(button.DataContext, parameters);
			};
		}

		public static void SetMethodName(Button element, string value)
		{
			element.SetValue(MethodNameProperty, value);
		}
		public static string GetMethodName(Button element)
		{
			return (string)element.GetValue(MethodNameProperty);
		}

		public static void SetParameter(Button element, object value)
		{
			element.SetValue(ParameterProperty, value);
		}
		public static object GetParameter(Button element)
		{
			return element.GetValue(ParameterProperty);
		}
	}
}
