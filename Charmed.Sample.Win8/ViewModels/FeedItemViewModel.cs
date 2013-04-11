using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed.Sample.ViewModels
{
	public sealed class FeedItemViewModel : ViewModelBase
	{
		private readonly IShareManager shareManager;

		public FeedItemViewModel(IShareManager shareManager)
		{
			this.shareManager = shareManager;

			this.shareManager.Initialize();
			this.shareManager.OnShareRequested = ShareRequested;
		}

		private void ShareRequested(DataPackage dataPackage)
		{
			// Set as many data types as we can.
			dataPackage.Properties.Title = this.FeedItem.Title;

			// Add a Uri
			dataPackage.SetUri(this.FeedItem.Link);

			// Add a text only version
			var text = string.Format("Check this out! {0} by {1} ({2})", this.FeedItem.Title, this.FeedItem.Author, this.FeedItem.Link);
			dataPackage.SetText(text);

			// Add an HTML version.
			var htmlBuilder = new StringBuilder();
			htmlBuilder.AppendFormat("<h1>{0}</h1>", this.FeedItem.Title);
			htmlBuilder.AppendFormat("<p>Check this out!</p>", this.FeedItem.Author);
			htmlBuilder.AppendFormat("<p><a href='{0}'>{1}</a> by {2}</p>", this.FeedItem.Link, this.FeedItem.Title, this.FeedItem.Author);
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
