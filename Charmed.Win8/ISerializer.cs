using System;

namespace Charmed
{
	public interface ISerializer
	{
		object Deserialize(Type type, string json);
		T Deserialize<T>(string json);
		string Serialize(object instance);
	}
}
