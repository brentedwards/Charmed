
namespace Charmed
{
	/// <summary>
	/// Interface for saving application settings and data.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Adds or updates setting data.
		/// </summary>
		/// <param name="key">The key for the data.</param>
		/// <param name="value">The data.</param>
		void AddOrUpdate(string key, object value);

		/// <summary>
		/// Tries to get setting data.
		/// </summary>
		/// <typeparam name="T">The type of the result.</typeparam>
		/// <param name="key">The key for the data.</param>
		/// <param name="value">The output data value.</param>
		/// <returns>Returns true if the key existed and the value was assigned to value.</returns>
		bool TryGetValue<T>(string key, out T value);

		/// <summary>
		/// Removes setting data.
		/// </summary>
		/// <param name="key">The key for the data.</param>
		/// <returns>Returns true if the data value existed and was removed and false if it did not exist.</returns>
		bool Remove(string key);
	}
}
