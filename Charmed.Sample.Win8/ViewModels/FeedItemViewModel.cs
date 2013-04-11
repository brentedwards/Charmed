using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Sample.ViewModels
{
	public sealed class FeedItemViewModel : ViewModelBase
	{
		private FeedItem feedItem;
		public FeedItem FeedItem
		{
			get { return this.feedItem; }
			set { this.SetProperty(ref this.feedItem, value); }
		}
	}
}
