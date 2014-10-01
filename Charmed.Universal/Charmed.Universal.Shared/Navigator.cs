using Charmed.Container;
using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Charmed
{
	public sealed class Navigator : INavigator
	{
		private readonly ISerializer serializer;
		private readonly IContainer container;

		public Navigator(
			ISerializer serializer,
			IContainer container)
		{
			this.serializer = serializer;
			this.container = container;
		}

		public void NavigateToViewModel<TViewModel>(object parameter = null)
		{
			var viewType = ResolveViewType<TViewModel>();
			var frame = (Frame)Window.Current.Content;

			if (parameter != null)
			{
				frame.Navigate(viewType, this.serializer.Serialize(parameter));
			}
			else
			{
				frame.Navigate(viewType);
			}
		}

		public void GoBack()
		{
			((Frame)Window.Current.Content).GoBack();
		}

		public bool CanGoBack
		{
			get
			{
				return ((Frame)Window.Current.Content).CanGoBack;
			}
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
