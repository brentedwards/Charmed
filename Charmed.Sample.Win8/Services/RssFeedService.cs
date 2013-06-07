using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charmed.Sample.Services
{
	/// <summary>
	/// Service to read RSS feeds.
	/// </summary>
	/// <remarks>
	/// The idea behind this class is mostly borrowed from Windows Store Blog Reader Sample
	/// http://msdn.microsoft.com/en-us/library/windows/apps/br211380.aspx
	/// </remarks>
	public abstract class RssFeedService : IRssFeedService
	{
		protected readonly ISettings settings;

		public RssFeedService(ISettings settings)
		{
			this.settings = settings;
		}

		public async Task<List<FeedData>> GetFeedsAsync()
		{
			List<string> feeds = null;
			string[] feedData;
			if (settings.TryGetValue<string[]>(Constants.FeedsKey, out feedData))
			{
				feeds = new List<string>(feedData);
			}
			else
			{
				throw new ArgumentException("There are no feeds");
			}

			var feedsData = new List<FeedData>();

			foreach (var feed in feeds)
			{
				var data = await GetFeedAsync(feed);
				if (data != null)
				{
					feedsData.Add(data);
				}
			}

			return feedsData;
		}

		protected abstract Task<FeedData> GetFeedAsync(string feedUriString);
	}
}
