using Charmed.Sample.Models;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed.Sample.ViewModels
{
	public sealed class FeedItemViewModel : SampleViewModelBase<FeedItem>
	{
		private readonly IShareManager shareManager;

		public FeedItemViewModel(IShareManager shareManager)
		{
			this.shareManager = shareManager;
		}

		public override void LoadState(FeedItem navigationParameter, Dictionary<string, object> pageState)
		{
			this.shareManager.Initialize();
			this.shareManager.OnShareRequested = ShareRequested;

			this.FeedItem = navigationParameter;
		}

		public override void SaveState(Dictionary<string, object> pageState)
		{
			this.shareManager.Cleanup();
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
	}
}
