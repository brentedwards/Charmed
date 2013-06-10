using Charmed.Helpers;
using Charmed.Messaging;
using Charmed.Sample.Messages;
using Charmed.Sample.Models;
using Charmed.Sample.Services;
using System.Collections.Generic;

namespace Charmed.Sample.ViewModels
{
	public sealed class MainViewModel : ViewModelBase
	{
		private readonly IRssFeedService rssFeedService;
		private readonly INavigator navigator;
		private readonly IMessageBus messageBus;

		public MainViewModel(
			IRssFeedService rssFeedService,
			INavigator navigator,
			IMessageBus messageBus)
		{
			this.rssFeedService = rssFeedService;
			this.navigator = navigator;
			this.messageBus = messageBus;

			this.messageBus.Subscribe<FeedsChangedMessage>((message) =>
				{
					LoadFeedData();
				});
		}

		public void ViewFeed(FeedItem feedItem)
		{
			this.navigator.NavigateToViewModel<FeedItemViewModel>(feedItem);
		}

		private void LoadFeedData()
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

		private List<FeedData> feedData;
		public List<FeedData> FeedData
		{
			get
			{
				if (this.feedData == null && !this.IsBusy)
				{
					LoadFeedData();
				}

				return this.feedData;
			}
			private set { this.SetProperty(ref this.feedData, value); }
		}
	}
}
