using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed.Helpers
{
	/// <summary>
	/// Helper class for routing a ListView's ItemClicks to a method on the view model.
	/// </summary>
	public sealed class ListViewHelper
	{
		public static readonly DependencyProperty ItemClickMethodNameProperty =
			DependencyProperty.RegisterAttached(
				"ItemClickMethodName",
				typeof(string),
				typeof(ListViewHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var listView = sender as ListViewBase;
					if (listView != null)
					{
						new ListViewHelper(listView);
					}
				})));

		public ListViewHelper(ListViewBase listView)
		{
			listView.ItemClick += (sender, args) =>
			{
				var methodName = GetItemClickMethodName(listView);

				var method = listView.DataContext.GetType().GetTypeInfo().GetDeclaredMethod(methodName);
				var parms = method.GetParameters();

				if (parms != null && parms.Length == 1)
				{
					method.Invoke(listView.DataContext, new[] { args.ClickedItem });
				}
				else
				{
					method.Invoke(listView.DataContext, null);
				}
			};

			listView.SelectionChanged += (sender, args) =>
			{
				var methodName = GetItemClickMethodName(listView);

				var dataContext = GetMethodContext(listView);
				if (dataContext == null)
				{
					dataContext = listView.DataContext;
				}

				var method = dataContext.GetType().GetTypeInfo().GetDeclaredMethod(methodName);
				var parms = method.GetParameters();

				if (parms != null && parms.Length == 1)
				{
					method.Invoke(dataContext, new[] { args.AddedItems[0] });
				}
				else
				{
					method.Invoke(dataContext, null);
				}
			};
		}

		public static void SetItemClickMethodName(
			DependencyObject attached,
			string itemClickMethodName)
		{
			attached.SetValue(ItemClickMethodNameProperty, itemClickMethodName);
		}

		public static string GetItemClickMethodName(DependencyObject attached)
		{
			return (string)attached.GetValue(ItemClickMethodNameProperty);
		}

		public static readonly DependencyProperty MethodContextProperty =
			DependencyProperty.RegisterAttached(
				"MethodContext",
				typeof(object),
				typeof(ListViewHelper),
				new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
				{
					var listView = sender as ListViewBase;
					if (listView != null)
					{
						if (!(listView.Tag is ListViewHelper))
						{
							listView.Tag = new ListViewHelper(listView);
						}
					}
				})));

		public static void SetMethodContext(ListViewBase element, object value)
		{
			element.SetValue(MethodContextProperty, value);
		}
		public static object GetMethodContext(ListViewBase element)
		{
			return element.GetValue(MethodContextProperty);
		}
	}
}
