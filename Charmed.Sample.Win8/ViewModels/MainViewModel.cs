using Charmed.Helpers;
using Charmed.Sample.Models;
using Charmed.Sample.Services;
using System.Collections.Generic;

namespace Charmed.Sample.ViewModels
{
	public sealed class MainViewModel : ViewModelBase
	{
		private readonly IRssFeedService rssFeedService;
		private readonly INavigator navigator;

		public MainViewModel(
			IRssFeedService rssFeedService,
			INavigator navigator)
		{
			this.rssFeedService = rssFeedService;
			this.navigator = navigator;
		}

		public void ViewFeed(FeedItem feedItem)
		{
			this.navigator.NavigateToViewModel<FeedItemViewModel>(feedItem, "FeedItem");
		}

		private List<FeedData> feedData;
		public List<FeedData> FeedData
		{
			get
			{
				if (this.feedData == null && !this.IsBusy)
				{
					this.IsBusy = true;
					AsyncHelper.LoadData(
						(f) =>
						{
							this.FeedData = f;
							this.IsBusy = false;
						},
						() => this.rssFeedService.GetFeedsAsync());
				}

				return this.feedData;
			}
			private set { this.SetProperty(ref this.feedData, value); }
		}
	}
}
