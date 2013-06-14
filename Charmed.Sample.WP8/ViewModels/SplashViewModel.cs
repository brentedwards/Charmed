using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Sample.ViewModels
{
	public sealed class SplashViewModel : ViewModelBase<int?>
	{
		private readonly IStorage storage;
		private readonly INavigator navigator;

		public SplashViewModel(
			IStorage storage,
			INavigator navigator,
			ISerializer serializer)
			: base(serializer)
		{
			this.storage = storage;
			this.navigator = navigator;
		}

		public override async void LoadState(int? navigationParameter, Dictionary<string, object> pageState)
		{
			this.navigator.NavigateToViewModel<MainViewModel>();
			this.navigator.RemoveBackEntry();

			if (navigationParameter.HasValue)
			{
				List<FeedItem> pinnedFeedItems = await storage.LoadAsync<List<FeedItem>>(Constants.PinnedFeedItemsKey);
				if (pinnedFeedItems != null)
				{
					var pinnedFeedItem = pinnedFeedItems.FirstOrDefault(fi => fi.Id == navigationParameter.Value);
					if (pinnedFeedItem != null)
					{
						this.navigator.NavigateToViewModel<FeedItemViewModel>(pinnedFeedItem);
					}
				}
			}
		}
	}
}
