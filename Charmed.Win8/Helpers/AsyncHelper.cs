using System;
using System.Threading.Tasks;

namespace Charmed.Helpers
{
	/// <summary>
	/// Helper class for calling async methods from synchronous methods.
	/// </summary>
	public static class AsyncHelper
	{
		/// <summary>
		/// Loads data asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the data being loaded</typeparam>
		/// <param name="callback">The Action that will receive the result</param>
		/// <param name="loader">The Func that will load the data</param>
		public static void LoadData<T>(Action<T> callback, Func<Task<T>> loader)
		{
			var task = loader();
			var awaiter = task.GetAwaiter();
			awaiter.OnCompleted(() =>
			{
				if (task.Exception != null)
				{
					throw task.Exception;
				}
				else
				{
					callback(task.Result);
				}
			});
		}
	}
}
