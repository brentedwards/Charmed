using Charmed.Sample.Models;
using System;
using System.Threading.Tasks;

namespace Charmed.Sample.ViewModels
{
	public sealed class WP8FeedItemViewModel : FeedItemViewModel
	{
		public WP8FeedItemViewModel(
			ISerializer serializer,
			IStorage storage,
			ISecondaryPinner secondaryPinner)
			: base(serializer, storage, secondaryPinner)
		{
		}

		public async Task Pin()
		{
			// Pin the feed item, then save it locally to make sure it is still available
			// when they return.
			var tileInfo = new TileInfo(
				this.FormatTileIdUrl(),
				this.FeedItem.Title,
				Constants.AppName,
				new Uri("/Assets/ApplicationIcon.png", UriKind.Relative));

			this.IsFeedItemPinned = await this.secondaryPinner.Pin(tileInfo);

			if (this.IsFeedItemPinned)
			{
				await SavePinnedFeedItem();
			}
		}

		public async Task Unpin()
		{
			// Unpin, then delete the feed item locally.
			var tileInfo = new TileInfo(this.FormatTileIdUrl());
			this.IsFeedItemPinned = !await this.secondaryPinner.Unpin(tileInfo);

			if (!this.IsFeedItemPinned)
			{
				await RemovePinnedFeedItem();
			}
		}

		private string FormatTileIdUrl()
		{
			var queryString = string.Format("parameter={0}", FeedItem.Id);
			return string.Format(Constants.SecondaryUriFormat, queryString);
		}
	}
}
