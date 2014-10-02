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

		public Func<string, object, object> TryGetValueDelegate { get; set; }
		public bool TryGetValue<T>(string key, out T value)
		{
			value = default(T);
			value = (T)MockHelper.ExecuteDelegate<string, object, object>(this.TryGetValueDelegate, key, value);
			return !object.Equals(value, default(T));
		}

		public Func<string, bool> RemoveDelegate { get; set; }
		public bool Remove(string key)
		{
			return MockHelper.ExecuteDelegate<string, bool>(this.RemoveDelegate, key);
		}


		public Func<string, bool> ContainsKeyDelegate { get; set; }
		public bool ContainsKey(string key)
		{
			return MockHelper.ExecuteDelegate<string, bool>(this.ContainsKeyDelegate, key);
		}
	}
}
