using Charmed.Helpers;
using Charmed.Sample.Models;
using Charmed.Sample.Services;
using System.Collections.Generic;

namespace Charmed.Sample.ViewModels
{
	public sealed class MainViewModel : ViewModelBase
	{
		private readonly IRssFeedService rssFeedService;

		public MainViewModel(IRssFeedService rssFeedService)
		{
			this.rssFeedService = rssFeedService;
		}

		private bool isFeedDataLoading = false;
		private List<FeedData> feedData;
		public List<FeedData> FeedData
		{
			get
			{
				if (this.feedData == null && !this.isFeedDataLoading)
				{
					this.isFeedDataLoading = true;
					AsyncHelper.LoadData(
						(f) => this.FeedData = f,
						() => this.rssFeedService.GetFeedsAsync());
				}

				return this.feedData;
			}
			private set
			{
				this.feedData = value;
				this.NotifyPropertyChanged("FeedData");
			}
		}
	}
}
