using System.Threading.Tasks;

namespace Charmed
{
	/// <summary>
	/// Interface for saving data to local storage.
	/// </summary>
	public interface IStorage
	{
		/// <summary>
		/// Loads data from local storage with the given file name.
		/// </summary>
		/// <typeparam name="T">The type of data to load.</typeparam>
		/// <param name="fileName">The name of the file to load data from.</param>
		/// <returns>Returns the loaded data or null if the file was not found.</returns>
		Task<T> LoadAsync<T>(string fileName);

		/// <summary>
		/// Saves data to local storage with the given file name.
		/// </summary>
		/// <param name="fileName">The name fo the file to save data to.</param>
		/// <param name="data">The data to save.</param>
		/// <returns></returns>
		Task SaveAsync(string fileName, object data);
	}
}
