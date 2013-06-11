using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Charmed.Sample.Services
{
	public sealed class WP8RssFeedService : RssFeedService
	{
		public WP8RssFeedService(ISettings settings)
			: base(settings)
		{
		}

		protected override Task<Models.FeedData> GetFeedAsync(string feedUriString)
		{
			var task = new TaskCompletionSource<Models.FeedData>();

			var webClient = new WebClient();
			webClient.DownloadStringCompleted += (sender, args) =>
			{
				if (args.Error == null)
				{
					var stringReader = new StringReader(args.Result);
					var feedData = new FeedData();
					using (var xmlReader = XmlReader.Create(stringReader))
					{
						var feed = SyndicationFeed.Load(xmlReader);

						if (feed.Title != null && feed.Title.Text != null)
						{
							feedData.Title = feed.Title.Text;
						}
						if (feed.Description != null && feed.Description.Text != null)
						{
							feedData.Description = feed.Description.Text;
						}
						if (feed.Items != null && feed.Items.Any())
						{
							feedData.PublishedDate = feed.Items.First().PublishDate.DateTime;

							foreach (var item in feed.Items)
							{
								var feedItem = new FeedItem();
								if (item.Title != null && item.Title.Text != null)
								{
									feedItem.Title = item.Title.Text;
								}
								if (item.PublishDate != null)
								{
									feedItem.PublishedDate = item.PublishDate.DateTime;
								}
								if (item.Authors != null && item.Authors.Count > 0)
								{
									feedItem.PublishedDate = item.PublishDate.DateTime;
								}
								if (item.Content != null && ((TextSyndicationContent)item.Content).Text != null)
								{
									feedItem.Content = ((TextSyndicationContent)item.Content).Text;
								}
								else if (item.Summary != null && item.Summary.Text != null)
								{
									feedItem.Content = item.Summary.Text;
								}
								if (item.Links != null && item.Links.Count > 0)
								{
									feedItem.Link = item.Links[0].Uri;
								}

								feedItem.Id = Regex.Replace(feedItem.Link.ToString(), @"[^0-9a-zA-Z\.]+", string.Empty).GetHashCode();

								feedData.Items.Add(feedItem);
							}
						}
					}
					task.TrySetResult(feedData);
				}
				else
				{
					task.TrySetException(args.Error);
				}
			};
			webClient.DownloadStringAsync(new Uri(feedUriString));

			return task.Task;
		}
	}
}
