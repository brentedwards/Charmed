using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Sample.Models
{
	public sealed class FeedItem
	{
		public string Title { get; set; }
		public string Author { get; set; }
		public string Content { get; set; }
		public DateTime PublishedDate { get; set; }
		public Uri Link { get; set; }
	}
}
