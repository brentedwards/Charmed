using Charmed.Container;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Reflection;

namespace Charmed
{
	public sealed class Navigator : INavigator
	{
		private readonly ISerializer serializer;
		private readonly IContainer container;

		public Navigator(ISerializer serializer, IContainer container)
		{
			this.serializer = serializer;
			this.container = container;
		}

		public void NavigateToViewModel<TViewModel>(object parameter = null)
		{
			var viewType = ResolveViewType<TViewModel>();

			var frame = (Frame)Window.Current.Content;

			Windows.UI.Xaml.Navigation.NavigatedEventHandler onNavigated = (sender, args) =>
				{
					var page = args.Content as Page;
					page.DataContext = this.container.Resolve<TViewModel>();

					if (args.Parameter != null)
					{
						var json = args.Parameter.ToString();

						var viewModelType = typeof(TViewModel);
						var navProperty = viewModelType.GetTypeInfo().GetDeclaredProperty("NavigationParameter");

						if (navProperty != null)
						{
							var navPropertyType = navProperty.PropertyType;

							var navParam = this.serializer.Deserialize(navPropertyType, json);
							navProperty.SetValue(page.DataContext, navParam);
						}
					}
				};

			frame.Navigated += onNavigated;
			frame.Navigate(viewType, this.serializer.Serialize(parameter));
			frame.Navigated -= onNavigated;
		}

		public void GoBack()
		{
			((Frame)Window.Current.Content).GoBack();
		}

		public bool CanGoBack
		{
			get { return ((Frame)Window.Current.Content).CanGoBack; }
		}

		private static Type ResolveViewType<TViewModel>()
		{
			var viewModelType = typeof(TViewModel);

			var viewName = viewModelType.AssemblyQualifiedName.Replace(
				viewModelType.Name,
				viewModelType.Name.Replace("ViewModel", "Page"));

			return Type.GetType(viewName.Replace("Model", string.Empty));
		}
	}
}
