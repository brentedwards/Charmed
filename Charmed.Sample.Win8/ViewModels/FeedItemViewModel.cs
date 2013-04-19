using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using System.Linq;

namespace Charmed.Sample.ViewModels
{
	public sealed class FeedItemViewModel : ViewModelBase<FeedItem>
	{
		private readonly IShareManager shareManager;
		private readonly ISecondaryPinner secondaryPinner;
		private readonly IStorage storage;

		public FeedItemViewModel(
			IShareManager shareManager,
			ISerializer serializer,
			ISecondaryPinner secondaryPinner,
			IStorage storage)
			: base(serializer)
		{
			this.shareManager = shareManager;
			this.secondaryPinner = secondaryPinner;
			this.storage = storage;
		}

		public override void LoadState(FeedItem navigationParameter, Dictionary<string, object> pageState)
		{
			this.shareManager.Initialize();
			this.shareManager.OnShareRequested = ShareRequested;

			this.FeedItem = navigationParameter;

			this.IsFeedItemPinned = this.secondaryPinner.IsPinned(FormatSecondaryTileId());
		}

		public override void SaveState(Dictionary<string, object> pageState)
		{
			this.shareManager.Cleanup();
		}

		public async Task Pin(FrameworkElement anchorElement)
		{
			// Pin the feed item, then save it locally to make sure it is still available
			// when they return.
			var tileInfo = new TileInfo(
				FormatSecondaryTileId(),
				this.FeedItem.Title,
				this.FeedItem.Title,
				TileOptions.ShowNameOnLogo | TileOptions.ShowNameOnWideLogo,
				new Uri("ms-appx:///Assets/Logo.png"),
				new Uri("ms-appx:///Assets/WideLogo.png"),
				this.FeedItem.Id.ToString());

			this.IsFeedItemPinned = await this.secondaryPinner.Pin(
				anchorElement,
				Windows.UI.Popups.Placement.Above,
				tileInfo);

			if (this.IsFeedItemPinned)
			{
				await SavePinnedFeedItem();
			}
		}

		public async Task Unpin(FrameworkElement anchorElement)
		{
			// Unpin, then delete the feed item locally.
			this.IsFeedItemPinned = !await this.secondaryPinner.Unpin(
				anchorElement,
				Windows.UI.Popups.Placement.Above,
				this.FormatSecondaryTileId());

			if (!this.IsFeedItemPinned)
			{
				await RemovePinnedFeedItem();
			}
		}

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
