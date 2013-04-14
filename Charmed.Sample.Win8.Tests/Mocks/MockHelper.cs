using System;
using System.Threading.Tasks;

namespace Charmed.Sample.Tests.Mocks
{
	public static class MockHelper
	{
		public static TResult ExecuteDelegate<TResult>(Func<TResult> @delegate)
		{
			if (@delegate != null)
			{
				return @delegate();
			}
			else
			{
				return default(TResult);
			}
		}

		public static TResult ExecuteDelegate<TParam1, TResult>(Func<TParam1, TResult> @delegate, TParam1 param1)
		{
			if (@delegate != null)
			{
				return @delegate(param1);
			}
			else
			{
				return default(TResult);
			}
		}

		public static TResult ExecuteDelegate<TParam1, TParam2, TResult>(Func<TParam1, TParam2, TResult> @delegate, TParam1 param1, TParam2 param2)
		{
			if (@delegate != null)
			{
				return @delegate(param1, param2);
			}
			else
			{
				return default(TResult);
			}
		}

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
