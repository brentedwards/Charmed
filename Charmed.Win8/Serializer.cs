using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Charmed
{
	public sealed class Serializer : ISerializer
	{
		public object Deserialize(Type type, string json)
		{
			var decoded = System.Net.WebUtility.UrlDecode(json);
			var bytes = Encoding.Unicode.GetBytes(decoded);
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				var serializer = new DataContractJsonSerializer(type);
				return serializer.ReadObject(stream);
			}
		}

		public T Deserialize<T>(string json)
		{
			return (T)Deserialize(typeof(T), json);
		}

		public string Serialize(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			using (MemoryStream stream = new MemoryStream())
			{
				var serializer = new DataContractJsonSerializer(instance.GetType());
				serializer.WriteObject(stream, instance);
				stream.Position = 0;
				using (StreamReader reader = new StreamReader(stream))
				{
					var serialized = reader.ReadToEnd();
					var urlEncoded = System.Net.WebUtility.UrlEncode(serialized);
					return urlEncoded;
				}
			}
		}
	}
}
