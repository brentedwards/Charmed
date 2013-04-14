using System;
using System.Threading.Tasks;

namespace Charmed.Sample.Tests.Mocks
{
	public static class MockHelper
	{
		public static Task<TResult> ExecuteDelegateAsync<TResult>(Func<TResult> @delegate)
		{
			if (@delegate != null)
			{
				return Task.FromResult<TResult>(@delegate());
			}
			else
			{
				return Task.FromResult<TResult>(default(TResult));
			}
		}
	}
}
