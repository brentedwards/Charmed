using Charmed.Sample.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charmed.Sample.ViewModels
{
	public class FeedItemViewModel : ViewModelBase<FeedItem>
	{
		private readonly IStorage storage;
		protected readonly ISecondaryPinner secondaryPinner;

		public FeedItemViewModel(
			ISerializer serializer,
			IStorage storage,
			ISecondaryPinner secondaryPinner)
			: base(serializer)
		{
			this.storage = storage;
			this.secondaryPinner = secondaryPinner;
		}

		public override void LoadState(FeedItem navigationParameter, Dictionary<string, object> pageState)
		{
			this.FeedItem = navigationParameter;
		}

		protected async Task SavePinnedFeedItem()
		{
			var pinnedFeedItems = await this.storage.LoadAsync<List<FeedItem>>(Constants.PinnedFeedItemsKey);

			if (pinnedFeedItems == null)
			{
				pinnedFeedItems = new List<FeedItem>();
			}

			pinnedFeedItems.Add(feedItem);
			await this.storage.SaveAsync(Constants.PinnedFeedItemsKey, pinnedFeedItems);
		}

		protected async Task RemovePinnedFeedItem()
		{
			var pinnedFeedItems = await this.storage.LoadAsync<List<FeedItem>>(Constants.PinnedFeedItemsKey);

			if (pinnedFeedItems != null)
			{
				var pinnedFeedItem = pinnedFeedItems.FirstOrDefault(fi => fi.Id == this.FeedItem.Id);
				if (pinnedFeedItem != null)
				{
					pinnedFeedItems.Remove(pinnedFeedItem);
				}

				await this.storage.SaveAsync(Constants.PinnedFeedItemsKey, pinnedFeedItems);
			}
		}

		private FeedItem feedItem;
		public FeedItem FeedItem
		{
			get { return this.feedItem; }
			set { this.SetProperty(ref this.feedItem, value); }
		}

		private bool isFeedItemPinned;
		public bool IsFeedItemPinned
		{
			get { return this.isFeedItemPinned; }
			set { this.SetProperty(ref this.isFeedItemPinned, value); }
		}
	}
}
