
namespace Charmed
{
	public interface ISettings
	{
		void Add(string key, object value);
		bool TryGetValue<T>(string key, out T value);
		bool Remove(string key);
	}
}
