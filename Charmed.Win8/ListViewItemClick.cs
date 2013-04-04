using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed.Win8
{
	public static class ListViewItemClick
	{
		public static readonly DependencyProperty MethodNameProperty =
			DependencyProperty.RegisterAttached("MethodName", typeof(string), typeof(ListViewItemClick), new PropertyMetadata(null, MethodNamePropertyChanged));

		public static void SetMethodName(DependencyObject attached, string methodName)
		{
			if (attached == null)
			{
				throw new ArgumentNullException("attached");
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}

			attached.SetValue(MethodNameProperty, methodName);
		}

		public static string GetMethodName(DependencyObject attached)
		{
			if (attached == null)
			{
				throw new ArgumentNullException("attached");
			}

			return (string)attached.GetValue(MethodNameProperty);
		}

		private static void MethodNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
			var methodName = GetMethodName(listView);

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
