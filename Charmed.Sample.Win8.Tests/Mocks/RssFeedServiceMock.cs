using Charmed.Sample.Models;
using Charmed.Sample.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charmed.Sample.Tests.Mocks
{
	public class RssFeedServiceMock : IRssFeedService
	{
		public Func<List<FeedData>> GetFeedsAsyncDelegate { get; set; }
		public Task<List<FeedData>> GetFeedsAsync()
		{
			return MockHelper.ExecuteDelegateAsync(this.GetFeedsAsyncDelegate);
		}
	}
}
