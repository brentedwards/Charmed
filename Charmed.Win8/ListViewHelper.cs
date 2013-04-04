using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed
{
	public class ListViewHelper
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
	}
}
