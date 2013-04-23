using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Charmed.Sample.Services
{
	/// <summary>
	/// Service to read RSS feeds.
	/// </summary>
	/// <remarks>
	/// The idea behind this class is mostly borrowed from Windows Store Blog Reader Sample
	/// http://msdn.microsoft.com/en-us/library/windows/apps/br211380.aspx
	/// </remarks>
	public sealed class RssFeedService : IRssFeedService
	{
		private List<string> Feeds { get; set; }

		public RssFeedService(ISettings settings)
		{
			string[] feeds;
			if (settings.TryGetValue<string[]>(Constants.FeedsKey, out feeds))
			{
				this.Feeds = new List<string>(feeds);
			}
		}

		public async Task<List<FeedData>> GetFeedsAsync()
		{
			var feedsData = new List<FeedData>();

			foreach (var feed in this.Feeds)
			{
				feedsData.Add(await GetFeedAsync(feed));
			}

			return feedsData;
		}

		private async Task<FeedData> GetFeedAsync(string feedUriString)
		{
			var client = new SyndicationClient();
			var feedUri = new Uri(feedUriString);

			try
			{
				var feed = await client.RetrieveFeedAsync(feedUri);
				var feedData = new FeedData();
				if (feed.Title != null && feed.Title.Text != null)
				{
					feedData.Title = feed.Title.Text;
				}
				if (feed.Subtitle != null && feed.Subtitle.Text != null)
				{
					feedData.Description = feed.Subtitle.Text;
				}
				if (feed.Items != null && feed.Items.Count > 0)
				{
					feedData.PublishedDate = feed.Items[0].PublishedDate.DateTime;

					foreach (var item in feed.Items)
					{
						var feedItem = new FeedItem();
						if (item.Title != null && item.Title.Text != null)
						{
							feedItem.Title = item.Title.Text;
						}
						if (item.PublishedDate != null)
						{
							feedItem.PublishedDate = item.PublishedDate.DateTime;
						}
						if (item.Authors != null && item.Authors.Count > 0)
						{
							feedItem.PublishedDate = item.PublishedDate.DateTime;
						}
						if (feed.SourceFormat == SyndicationFormat.Atom10)
						{
							if (item.Content != null && item.Content.Text != null)
							{
								feedItem.Content = item.Content.Text;
							}
							if (item.Id != null)
							{
								feedItem.Link = new Uri(item.Id);
							}
						}
						else if (feed.SourceFormat == SyndicationFormat.Rss20)
						{
							if (item.Summary != null && item.Summary.Text != null)
							{
								feedItem.Content = item.Summary.Text;
							}
							if (item.Links != null && item.Links.Count > 0)
							{
								feedItem.Link = item.Links[0].Uri;
							}
						}

						feedItem.Id = Regex.Replace(feedItem.Link.ToString(), @"[^0-9a-zA-Z\.]+", string.Empty).GetHashCode();

						feedData.Items.Add(feedItem);
					}
				}

				return feedData;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return null;
			}
		}
	}
}
