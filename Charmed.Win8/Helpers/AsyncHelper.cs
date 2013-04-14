using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#if NETFX_CORE
using System.Collections.Concurrent;
#endif // NETFX_CORE

namespace Charmed.Helpers
{
	/// <summary>
	/// Helper class for calling async methods from synchronous methods.
	/// </summary>
	/// <remarks>
	/// The WinRT version of this implementation is borrowed from Stephen Toub.
	/// http://blogs.msdn.com/b/pfxteam/archive/2012/01/20/10259049.aspx
	/// </remarks>
	public static class AsyncHelper
	{
		/// <summary>
		/// Loads data asynchronously, calling the callback when completed.
		/// </summary>
		/// <typeparam name="T">The type of the data being loaded</typeparam>
		/// <param name="callback">The Action that will receive the result</param>
		/// <param name="loader">The Func that will load the data</param>
		public static void LoadData<T>(Action<T> callback, Func<Task<T>> loader)
		{
#if WINDOWS_PHONE
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
#else
			var prevCtx = SynchronizationContext.Current;
			try
			{
				var syncCtx = new SingleThreadSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(syncCtx);

				var t = loader();
				t.ContinueWith(
					delegate { syncCtx.Complete(); }, TaskScheduler.Default);

				syncCtx.RunOnCurrentThread();

				callback(t.GetAwaiter().GetResult());
			}
			finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
#endif // WINDOWS_PHONE
		}

#if NETFX_CORE
		/// <summary>Provides a SynchronizationContext that's single-threaded.</summary>
		private sealed class SingleThreadSynchronizationContext : SynchronizationContext
		{
			/// <summary>The queue of work items.</summary>
			private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> queue =
				new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

			/// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
			/// <param name="d">The System.Threading.SendOrPostCallback delegate to call.</param>
			/// <param name="state">The object passed to the delegate.</param>
			public override void Post(SendOrPostCallback d, object state)
			{
				if (d == null)
				{
					throw new ArgumentNullException("d");
				}
				this.queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
			}

			/// <summary>Not supported.</summary>
			public override void Send(SendOrPostCallback d, object state)
			{
				throw new NotSupportedException("Synchronously sending is not supported.");
			}

			/// <summary>Runs an loop to process all queued work items.</summary>
			public void RunOnCurrentThread()
			{
				foreach (var workItem in this.queue.GetConsumingEnumerable())
				{
					workItem.Key(workItem.Value);
				}
			}

			/// <summary>Notifies the context that no more work will arrive.</summary>
			public void Complete()
			{
				this.queue.CompleteAdding();
			}
		}
#endif // NETFX_CORE
	}
}
