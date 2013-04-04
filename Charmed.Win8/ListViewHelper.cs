using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed
{
	public static class ListViewHelper
	{
		public static readonly DependencyProperty ItemClickMethodNameProperty =
			DependencyProperty.RegisterAttached(
				"ItemClickMethodName",
				typeof(string),
				typeof(ListViewHelper),
				new PropertyMetadata(null, ItemClickMethodNamePropertyChanged));

		public static void SetItemClickMethodName(
			DependencyObject attached,
			string itemClickMethodName)
		{
			if (attached == null)
			{
				throw new ArgumentNullException("attached");
			}
			if (itemClickMethodName == null)
			{
				throw new ArgumentNullException("itemClickMethodName");
			}

			attached.SetValue(ItemClickMethodNameProperty, itemClickMethodName);
		}

		public static string GetItemClickMethodName(DependencyObject attached)
		{
			if (attached == null)
			{
				throw new ArgumentNullException("attached");
			}

			return (string)attached.GetValue(ItemClickMethodNameProperty);
		}

		private static void ItemClickMethodNamePropertyChanged(
			DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}

			((ListViewBase)d).ItemClick += OnItemClick;
		}

		private static void OnItemClick(object sender, ItemClickEventArgs e)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}

			var listView = (ListViewBase)sender;
			var methodName = GetItemClickMethodName(listView);

			var method = listView.DataContext.GetType().GetTypeInfo().GetDeclaredMethod(methodName);
			var parms = method.GetParameters();

			if (parms != null && parms.Length == 1)
			{
				method.Invoke(listView.DataContext, new[] { e.ClickedItem });
			}
			else
			{
				method.Invoke(listView.DataContext, null);
			}
		}
	}
}
