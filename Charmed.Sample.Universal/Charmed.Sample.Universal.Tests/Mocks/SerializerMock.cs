using System;

namespace Charmed.Sample.Tests.Mocks
{
	public class SerializerMock : ISerializer
	{
		public Func<Type, string, object> DeserializeDelegate { get; set; }
		public object Deserialize(Type type, string data)
		{
			return MockHelper.ExecuteDelegate<Type, string, object>(this.DeserializeDelegate, type, data);
		}

		public Func<string, object> DeserializeGenericDelegate { get; set; }
		public T Deserialize<T>(string data)
		{
			return (T)MockHelper.ExecuteDelegate<string, object>(this.DeserializeGenericDelegate, data);
		}

		public Func<object, string> SerializeDelegate { get; set; }
		public string Serialize(object instance)
		{
			return MockHelper.ExecuteDelegate<object, string>(this.SerializeDelegate, instance);
		}
	}
}
