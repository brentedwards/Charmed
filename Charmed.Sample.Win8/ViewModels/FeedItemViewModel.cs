using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed.Sample.ViewModels
{
	public sealed class FeedItemViewModel : ViewModelBase<FeedItem>
	{
#if NETFX_CORE
		private readonly IShareManager shareManager;
		private readonly ISecondaryPinner secondaryPinner;
#endif // NETFX_CORE

		private readonly IStorage storage;

		public FeedItemViewModel(
			ISerializer serializer,
			IStorage storage
#if NETFX_CORE
			, IShareManager shareManager,
			ISecondaryPinner secondaryPinner
#endif // NETFX_CORE
			)
			: base(serializer)
		{
			this.storage = storage;

#if NETFX_CORE
			this.shareManager = shareManager;
			this.secondaryPinner = secondaryPinner;
#endif // NETFX_CORE
		}

		public override void LoadState(FeedItem navigationParameter, Dictionary<string, object> pageState)
		{
			this.FeedItem = navigationParameter;

#if NETFX_CORE
			this.IsFeedItemPinned = this.secondaryPinner.IsPinned(FormatSecondaryTileId());

			this.shareManager.Initialize();
			this.shareManager.OnShareRequested = ShareRequested;
#endif // NETFX_CORE
		}

		public override void SaveState(Dictionary<string, object> pageState)
		{
#if NETFX_CORE
			this.shareManager.Cleanup();
#endif // NETFX_CORE
		}

#if NETFX_CORE
		public async Task Pin(Windows.UI.Xaml.FrameworkElement anchorElement)
		{
			// Pin the feed item, then save it locally to make sure it is still available
			// when they return.
			var tileInfo = new TileInfo(
				this.FormatSecondaryTileId(),
				this.FeedItem.Title,
				this.FeedItem.Title,
				Windows.UI.StartScreen.TileOptions.ShowNameOnLogo | Windows.UI.StartScreen.TileOptions.ShowNameOnWideLogo,
				new Uri("ms-appx:///Assets/Logo.png"),
				new Uri("ms-appx:///Assets/WideLogo.png"),
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
#endif // NETFX_CORE

		private string FormatSecondaryTileId()
		{
			return string.Format(Constants.SecondaryIdFormat, this.FeedItem.Id);
		}

		private async Task SavePinnedFeedItem()
		{
			var pinnedFeedItems = await this.storage.LoadAsync<List<FeedItem>>(Constants.PinnedFeedItemsKey);

			if (pinnedFeedItems == null)
			{
				pinnedFeedItems = new List<FeedItem>();
			}

			pinnedFeedItems.Add(feedItem);
			await this.storage.SaveAsync(Constants.PinnedFeedItemsKey, pinnedFeedItems);
		}

		private async Task RemovePinnedFeedItem()
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

#if NETFX_CORE
		private void ShareRequested(DataPackage dataPackage)
		{
			// Set as many data types as we can.
			dataPackage.Properties.Title = this.FeedItem.Title;

			// Add a Uri
			dataPackage.SetUri(this.FeedItem.Link);

			// Add a text only version
			var text = string.Format("Check this out! {0} ({1})", this.FeedItem.Title, this.FeedItem.Link);
			dataPackage.SetText(text);

			// Add an HTML version.
			var htmlBuilder = new StringBuilder();
			htmlBuilder.AppendFormat("<p>Check this out!</p>", this.FeedItem.Author);
			htmlBuilder.AppendFormat("<p><a href='{0}'>{1}</a></p>", this.FeedItem.Link, this.FeedItem.Title);
			var html = HtmlFormatHelper.CreateHtmlFormat(htmlBuilder.ToString());
			dataPackage.SetHtmlFormat(html);
		}
#endif // NETFX_CORE

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
