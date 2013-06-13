using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed.Sample.ViewModels
{
	public sealed class Win8FeedItemViewModel : FeedItemViewModel
	{
		private readonly IShareManager shareManager;

		public Win8FeedItemViewModel(
			ISerializer serializer,
			IStorage storage,
			ISecondaryPinner secondaryPinner,
			IShareManager shareManager)
			: base(serializer, storage, secondaryPinner)
		{
			this.shareManager = shareManager;
		}

		public override void LoadState(Models.FeedItem navigationParameter, Dictionary<string, object> pageState)
		{
			base.LoadState(navigationParameter, pageState);

			this.IsFeedItemPinned = this.secondaryPinner.IsPinned(FormatSecondaryTileId());

			this.shareManager.Initialize();
			this.shareManager.OnShareRequested = ShareRequested;
		}

		public override void SaveState(Dictionary<string, object> pageState)
		{
			base.SaveState(pageState);

			this.shareManager.Cleanup();
		}

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

		private string FormatSecondaryTileId()
		{
			return string.Format(Constants.SecondaryIdFormat, this.FeedItem.Id);
		}
	}
}
