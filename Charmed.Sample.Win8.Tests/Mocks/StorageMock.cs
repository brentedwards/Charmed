using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Sample.Tests.Mocks
{
	public class StorageMock : IStorage
	{
		public Func<string, object> LoadAsyncDelegate { get; set; }
		public Task<T> LoadAsync<T>(string fileName)
		{
			if (this.LoadAsyncDelegate != null)
			{
				return Task.FromResult<T>((T)this.LoadAsyncDelegate(fileName));
			}
			else
			{
				return Task.FromResult<T>(default(T));
			}
		}

		public Action<string, object> SaveAsyncDelegate { get; set; }
		public Task SaveAsync(string fileName, object data)
		{
			return Task.Run(() =>
				{
					if (this.SaveAsyncDelegate != null)
					{
						this.SaveAsyncDelegate(fileName, data);
					}
				});
		}
	}
}
