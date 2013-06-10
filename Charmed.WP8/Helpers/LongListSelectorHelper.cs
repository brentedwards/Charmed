using Microsoft.Phone.Controls;
using System.Reflection;
using System.Windows;

namespace Charmed.Helpers
{
	/// <summary>
	/// Helper class for routing a LongListSelector's SelectionChangeds to a method on the view model.
	/// </summary>
	public sealed class LongListSelectorHelper
	{
		public static readonly DependencyProperty SelectionChangedMethodNameProperty =
			DependencyProperty.RegisterAttached(
				"SelectionChangedMethodName",
				typeof(string),
				typeof(LongListSelectorHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var longListSelector = sender as LongListSelector;
					if (longListSelector != null)
					{
						if (!(longListSelector.Tag is LongListSelectorHelper))
						{
							new LongListSelectorHelper(longListSelector);
						}
					}
				})));

		public static readonly DependencyProperty MethodContextProperty =
			DependencyProperty.RegisterAttached(
				"MethodContext",
				typeof(object),
				typeof(LongListSelectorHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var longListSelector = sender as LongListSelector;
					if (longListSelector != null)
					{
						if (!(longListSelector.Tag is LongListSelectorHelper))
						{
							longListSelector.Tag = new LongListSelectorHelper(longListSelector);
						}
					}
				})));

		public LongListSelectorHelper(LongListSelector longListSelector)
		{
			longListSelector.SelectionChanged += (sender, args) =>
			{
				var methodName = GetSelectionChangedMethodName(longListSelector);

				var dataContext = GetMethodContext(longListSelector);
				if (dataContext == null)
				{
					dataContext = longListSelector.DataContext;
				}

				var method = dataContext.GetType().GetTypeInfo().GetDeclaredMethod(methodName);
				var parms = method.GetParameters();

				if (parms != null && parms.Length == 1)
				{
					method.Invoke(dataContext, new[] { longListSelector.SelectedItem });
				}
				else
				{
					method.Invoke(dataContext, null);
				}
			};
		}

		public static void SetSelectionChangedMethodName(
			LongListSelector element,
			string itemClickMethodName)
		{
			element.SetValue(SelectionChangedMethodNameProperty, itemClickMethodName);
		}

		public static string GetSelectionChangedMethodName(LongListSelector element)
		{
			return (string)element.GetValue(SelectionChangedMethodNameProperty);
		}

		public static void SetMethodContext(LongListSelector element, object value)
		{
			element.SetValue(MethodContextProperty, value);
		}
		public static object GetMethodContext(LongListSelector element)
		{
			return element.GetValue(MethodContextProperty);
		}
	}
}
