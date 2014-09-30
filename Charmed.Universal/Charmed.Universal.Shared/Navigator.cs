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

		private Uri ResolveViewUri(Type viewType, object parameter = null)
		{
			var queryString = string.Empty;
			if (parameter != null)
			{
				var serializedParameter = this.serializer.Serialize(parameter);
				queryString = string.Format("?parameter={0}", serializedParameter);
			}

			var match = System.Text.RegularExpressions.Regex.Match(viewType.FullName, @"\.Views.*");
			if (match == null || match.Captures.Count == 0)
			{
				throw new ArgumentException("Views must exist in Views namespace.");
			}
			var path = match.Captures[0].Value.Replace('.', '/');

			return new Uri(string.Format("{0}.xaml{1}", path, queryString), UriKind.Relative);
		}
	}
}
