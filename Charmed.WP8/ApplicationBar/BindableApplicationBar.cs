using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace Charmed.ApplicationBar
{
	/// <summary>
	/// Application Bar which allows Menu Items to be databound.
	/// </summary>
	public sealed class BindableApplicationBar : FrameworkElement, IApplicationBar
	{
		private readonly Microsoft.Phone.Shell.ApplicationBar applicationBar;

		public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(
			"MenuItems",
			typeof(IList),
			typeof(BindableApplicationBar),
			new PropertyMetadata(new ObservableCollection<object>(), OnBindableMenuItemsChanged));

		public BindableApplicationBar()
		{
			this.applicationBar = new Microsoft.Phone.Shell.ApplicationBar();
			this.applicationBar.StateChanged += OnStateChanged;

			this.buttons = new ObservableCollection<ApplicationBarIconButton>();
			this.buttons.CollectionChanged += OnButtonsCollectionChanged;

			this.Loaded += OnLoaded;
		}

		private void OnMenuItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var item in e.NewItems)
				{
					var menuItem = GetMenuItem(this, item);
					this.applicationBar.MenuItems.Add(menuItem);
				}
			}
		}

		private void OnButtonsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var item in e.NewItems)
				{
					this.applicationBar.Buttons.Add(item);
				}
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			var page = BindableApplicationBar.FindRootPage(this);
			if (page != null)
			{
				page.ApplicationBar = this.applicationBar;
			}

			if (!AreMenuItemsLoaded && this.MenuItems != null)
			{
				foreach (var item in this.MenuItems)
				{
					var menuItem = GetMenuItem(this, item);
					this.applicationBar.MenuItems.Add(menuItem);
				}
			}

			foreach (var button in this.Buttons)
			{
				var bindableButton = button as BindableApplicationBarIconButton;
				if (bindableButton != null)
				{
					bindableButton.DataContext = this.DataContext;
				}
			}
		}

		private void OnStateChanged(object sender, ApplicationBarStateChangedEventArgs e)
		{
			if (this.StateChanged != null)
			{
				this.StateChanged(this, e);
			}
		}

		private static void OnBindableMenuItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}

			var bindableApplicationBar = (BindableApplicationBar)d;
			bindableApplicationBar.MenuItems.Clear();
			var items = e.NewValue as IList;
			if (items != null)
			{
				foreach (var item in items)
				{
					var menuItem = GetMenuItem(bindableApplicationBar, item);
					bindableApplicationBar.MenuItems.Add(menuItem);
				}
			}

			if (e.OldValue != null)
			{
				var oldObservableItems = e.NewValue as INotifyCollectionChanged;
				if (oldObservableItems != null)
				{
					oldObservableItems.CollectionChanged -= bindableApplicationBar.OnMenuItemsCollectionChanged;
				}
			}

			var observableItems = e.NewValue as INotifyCollectionChanged;
			if (observableItems != null)
			{
				observableItems.CollectionChanged += bindableApplicationBar.OnMenuItemsCollectionChanged;
			}
			bindableApplicationBar.AreMenuItemsLoaded = true;
		}

		private static object GetMenuItem(BindableApplicationBar bindableApplicationBar, object item)
		{
			var bindableMenuItem = item as BindableApplicationBarMenuItem;
			if (bindableMenuItem != null)
			{
				bindableMenuItem.DataContext = bindableMenuItem.DataContext;
				bindableMenuItem.DataItem = item;
				if (!string.IsNullOrEmpty(bindableApplicationBar.MenuItemClickMethodName))
				{
					bindableMenuItem.ClickMethodName = bindableApplicationBar.MenuItemClickMethodName;
				}
				return bindableMenuItem;
			}
			else if (item is ApplicationBarMenuItem)
			{
				return item;
			}
			else
			{
				var textProperty = item.GetType().GetProperty(bindableApplicationBar.TextMemberPath);
				var menuItem = new BindableApplicationBarMenuItem();
				menuItem.Text = textProperty.GetValue(item).ToString();
				menuItem.DataContext = bindableApplicationBar.DataContext;
				menuItem.ClickMethodName = bindableApplicationBar.MenuItemClickMethodName;
				menuItem.DataItem = item;
				return menuItem;
			}
		}

		private static PhoneApplicationPage FindRootPage(FrameworkElement control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}

			if (control != null && !(control is PhoneApplicationPage))
			{
				control = FindRootPage(control.Parent as FrameworkElement);
			}
			return control as PhoneApplicationPage;
		}

		public System.Windows.Media.Color BackgroundColor
		{
			get { return this.applicationBar.BackgroundColor; }
			set { this.applicationBar.BackgroundColor = value; }
		}

		private ObservableCollection<ApplicationBarIconButton> buttons;
		public System.Collections.IList Buttons
		{
			get { return buttons; }
		}

		public double DefaultSize
		{
			get { return this.applicationBar.DefaultSize; }
		}

		public System.Windows.Media.Color ForegroundColor
		{
			get { return this.applicationBar.ForegroundColor; }
			set { this.applicationBar.ForegroundColor = value; }
		}

		public bool IsMenuEnabled
		{
			get { return this.applicationBar.IsMenuEnabled; }
			set { this.applicationBar.IsMenuEnabled = value; }
		}

		public bool IsVisible
		{
			get { return this.applicationBar.IsVisible; }
			set { this.applicationBar.IsVisible = value; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public System.Collections.IList MenuItems
		{
			get { return (IList)this.GetValue(BindableApplicationBar.MenuItemsProperty); }
			set { this.SetValue(BindableApplicationBar.MenuItemsProperty, value); }
		}

		public double MiniSize
		{
			get { return this.applicationBar.MiniSize; }
		}

		public ApplicationBarMode Mode
		{
			get { return this.applicationBar.Mode; }
			set { this.applicationBar.Mode = value; }
		}

		public string TextMemberPath { get; set; }
		public string MenuItemClickMethodName { get; set; }

		private bool AreMenuItemsLoaded { get; set; }

		public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;
	}
}
