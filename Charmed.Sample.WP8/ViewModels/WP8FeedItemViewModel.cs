using Charmed.ApplicationBar;
using Charmed.Sample.Models;
using System;
using System.Collections.ObjectModel;
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
			this.AppBarButtons = new ObservableCollection<BindableApplicationBarIconButton>();

			this.PinButton = new BindableApplicationBarIconButton
			{
				IconUri = new Uri("/Assets/PinIcon.png", UriKind.Relative),
				Text = "pin",
				ClickMethodName = "Pin"
			};

			this.UnpinButton = new BindableApplicationBarIconButton
			{
				IconUri = new Uri("/Assets/UnpinIcon.png", UriKind.Relative),
				Text = "unpin",
				ClickMethodName = "Unpin"
			};
		}

		public override void LoadState(FeedItem navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
		{
			base.LoadState(navigationParameter, pageState);

			this.AppBarButtons.Clear();
			if (this.secondaryPinner.IsPinned(this.FormatTileIdUrl()))
			{
				this.AppBarButtons.Add(this.UnpinButton);
			}
			else
			{
				this.AppBarButtons.Add(this.PinButton);
			}
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
				await this.SavePinnedFeedItem();
			}
		}

		public async Task Unpin()
		{
			// Unpin, then delete the feed item locally.
			var tileInfo = new TileInfo(this.FormatTileIdUrl());
			this.IsFeedItemPinned = !await this.secondaryPinner.Unpin(tileInfo);

			if (!this.IsFeedItemPinned)
			{
				await this.RemovePinnedFeedItem();
			}
		}

		private string FormatTileIdUrl()
		{
			var queryString = string.Format("parameter={0}", FeedItem.Id);
			return string.Format(Constants.SecondaryUriFormat, queryString);
		}

		public ObservableCollection<BindableApplicationBarIconButton> AppBarButtons { get; private set; }

		private BindableApplicationBarIconButton PinButton { get; set; }
		private BindableApplicationBarIconButton UnpinButton { get; set; }
	}
}
