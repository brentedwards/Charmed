using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed.Sample.ViewModels
{
	public sealed class WindowsFeedItemViewModel : FeedItemViewModel
	{
		private readonly IShareManager shareManager;

		public WindowsFeedItemViewModel(
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

			this.shareManager.Initialize();
			this.shareManager.OnShareRequested = ShareRequested;
		}

		public override void SaveState(Dictionary<string, object> pageState)
		{
			base.SaveState(pageState);

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
	}
}
