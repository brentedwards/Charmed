using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Charmed
{
	public sealed class Storage : IStorage
	{
		private readonly ISerializer serializer;

		public Storage(ISerializer serializer)
		{
			this.serializer = serializer;
		}

		public async Task<T> Load<T>(string fileName)
		{
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

			var serializedData = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);

			T data = default(T);
			if (!string.IsNullOrWhiteSpace(serializedData))
			{
				data = this.serializer.Deserialize<T>(serializedData);
			}

			return data;
		}

		public async Task Save(string fileName, object data)
		{
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

			var serializedData = this.serializer.Serialize(data);

			await FileIO.WriteTextAsync(file, serializedData, Windows.Storage.Streams.UnicodeEncoding.Utf8);
		}
	}
}
