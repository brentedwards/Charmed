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
	/// <summary>
	/// Helper class for routing a Button's Clicks to a method on the view model.
	/// </summary>
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

		public static readonly DependencyProperty MethodContextProperty =
			DependencyProperty.RegisterAttached(
				"MethodContext",
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

				var dataContext = GetMethodContext(button);
				if (dataContext == null)
				{
					dataContext = button.DataContext;
				}

				var typeInfo = dataContext.GetType().GetTypeInfo();
				var method = typeInfo.GetDeclaredMethod(methodName);
				while (method == null && typeInfo.BaseType != null)
				{
					typeInfo = typeInfo.BaseType.GetTypeInfo();
					method = typeInfo.GetDeclaredMethod(methodName);
				}

				if (method == null)
				{
					throw new ArgumentException(string.Format("Method '{0}' not found on type '{1}'.", methodName, dataContext.GetType().Name));
				}
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

				method.Invoke(dataContext, parameters);
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

		public static void SetMethodContext(Button element, object value)
		{
			element.SetValue(MethodContextProperty, value);
		}
		public static object GetMethodContext(Button element)
		{
			return element.GetValue(MethodContextProperty);
		}
	}
}
