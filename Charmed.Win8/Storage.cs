using System;
using System.IO;
using System.Text;
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

		public async Task<T> LoadAsync<T>(string fileName)
		{
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

			string serializedData = null;
			using (var stream = await file.OpenStreamForReadAsync())
			{
				using (var streamReader = new StreamReader(stream))
				{
					serializedData = await streamReader.ReadToEndAsync();
				}
			}

			T data = default(T);
			if (!string.IsNullOrWhiteSpace(serializedData))
			{
				data = this.serializer.Deserialize<T>(serializedData);
			}

			return data;
		}

		public async Task SaveAsync(string fileName, object data)
		{
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

			var serializedData = this.serializer.Serialize(data);

			using (var stream = await file.OpenStreamForWriteAsync())
			{
				byte[] fileBytes = Encoding.UTF8.GetBytes(serializedData.ToCharArray());
				await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
			}
		}
	}
}
