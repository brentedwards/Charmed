using System;

namespace Charmed.Sample.Models
{
	public sealed class FeedItem
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Content { get; set; }
		public DateTime PublishedDate { get; set; }
		public Uri Link { get; set; }
	}
}
