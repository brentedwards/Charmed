using System;

namespace Charmed.Sample.Tests.Mocks
{
	public class SettingsMock : ISettings
	{
		public Action<string, object> AddOrUpdateDelegate { get; set; }
		public void AddOrUpdate(string key, object value)
		{
			if (this.AddOrUpdateDelegate != null)
			{
				this.AddOrUpdateDelegate(key, value);
			}
		}

		public Func<string, object, bool> TryGetValueDelegate { get; set; }
		public bool TryGetValue<T>(string key, out T value)
		{
			value = default(T);
			return MockHelper.ExecuteDelegate<string, object, bool>(this.TryGetValueDelegate, key, value);
		}

		public Func<string, bool> RemoveDelegate { get; set; }
		public bool Remove(string key)
		{
			return MockHelper.ExecuteDelegate<string, bool>(this.RemoveDelegate, key);
		}
	}
}
