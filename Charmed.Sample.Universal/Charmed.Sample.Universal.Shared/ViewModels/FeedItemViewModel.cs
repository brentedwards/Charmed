using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

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

			this.IsFeedItemPinned = this.secondaryPinner.IsPinned(this.FormatSecondaryTileId());
		}

		public async Task Pin(Windows.UI.Xaml.FrameworkElement anchorElement)
		{
			// Pin the feed item, then save it locally to make sure it is still available
			// when they return.
			var tileInfo = new TileInfo(
				this.FormatSecondaryTileId(),
				this.FeedItem.Title,
				new Uri("ms-appx:///Assets/Logo.png"),
				new Uri("ms-appx:///Assets/WideLogo.png"),
				TileSize.Square150x150,
				anchorElement,
				Windows.UI.Popups.Placement.Above,
				this.FeedItem.Id.ToString());

			this.IsFeedItemPinned = await this.secondaryPinner.Pin(tileInfo);

			if (this.IsFeedItemPinned)
			{
				await SavePinnedFeedItem();
			}
		}

		public async Task Unpin(Windows.UI.Xaml.FrameworkElement anchorElement)
		{
			// Unpin, then delete the feed item locally.
			var tileInfo = new TileInfo(this.FormatSecondaryTileId(), anchorElement, Windows.UI.Popups.Placement.Above);
			this.IsFeedItemPinned = !await this.secondaryPinner.Unpin(tileInfo);

			if (!this.IsFeedItemPinned)
			{
				await RemovePinnedFeedItem();
			}
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

		private string FormatSecondaryTileId()
		{
			return string.Format(Constants.SecondaryIdFormat, this.FeedItem.Id);
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
