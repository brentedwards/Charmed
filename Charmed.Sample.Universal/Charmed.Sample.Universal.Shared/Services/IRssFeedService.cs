using Charmed.Sample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charmed.Sample.Services
{
	/// <summary>
	/// Interface for reading RSS feeds.
	/// </summary>
	public interface IRssFeedService
	{
		Task<List<FeedData>> GetFeedsAsync();
	}
}
