#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
#endif // WINDOWS_PHONE

using Windows.Storage;

namespace Charmed
{
	public sealed class Settings : ISettings
	{
		public void Add(string key, object value)
		{
#if WINDOWS_PHONE
			IsolatedStorageSettings.ApplicationSettings.Add(key, value);
			IsolatedStorageSettings.ApplicationSettings.Save();
#else
			ApplicationData.Current.RoamingSettings.Values[key] = value;
#endif // WINDOWS_PHONE
		}

		public bool TryGetValue<T>(string key, out T value)
		{
#if WINDOWS_PHONE
			return IsolatedStorageSettings.ApplicationSettings.TryGetValue<T>(key, out value);
#else
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
#endif // WINDOWS_PHONE
		}


		public bool Remove(string key)
		{
#if WINDOWS_PHONE
			var result = IsolatedStorageSettings.ApplicationSettings.Remove(key);
			IsolatedStorageSettings.ApplicationSettings.Save();
			return result;
#else
			return ApplicationData.Current.RoamingSettings.Values.Remove(key);
#endif // WINDOWS_PHONE
		}
	}
}
