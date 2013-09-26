#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
using Windows.Storage;

namespace Charmed
{
	public sealed class Settings : ISettings
	{
		public void AddOrUpdate(string key, object value)
		{
			IsolatedStorageSettings.ApplicationSettings[key] = value;
			IsolatedStorageSettings.ApplicationSettings.Save();
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			return IsolatedStorageSettings.ApplicationSettings.TryGetValue<T>(key, out value);

		}


		public bool Remove(string key)
		{
			var result = IsolatedStorageSettings.ApplicationSettings.Remove(key);
			IsolatedStorageSettings.ApplicationSettings.Save();
			return result;
		}

		public bool ContainsKey(string key)
		{
			return IsolatedStorageSettings.ApplicationSettings.Contains(key);
		}
	}
}

#else

using Windows.Storage;

namespace Charmed
{
	public sealed class Settings : ISettings
	{
		public void AddOrUpdate(string key, object value)
		{
			ApplicationData.Current.RoamingSettings.Values[key] = value;
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			var result = false;
			if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
			{
				value = (T)ApplicationData.Current.RoamingSettings.Values[key];
				result = true;
			}
			else
			{
				value = default(T);
			}

			return result;
		}


		public bool Remove(string key)
		{
			return ApplicationData.Current.RoamingSettings.Values.Remove(key);
		}

		public bool ContainsKey(string key)
		{
			return ApplicationData.Current.RoamingSettings.Values.ContainsKey(key);
		}
	}
}
#endif // WINDOWS_PHONE
