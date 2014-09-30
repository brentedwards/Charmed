using System;

namespace Charmed
{
	/// <summary>
	/// An interface for serializing and deserializing objects.
	/// </summary>
	public interface ISerializer
	{
		/// <summary>
		/// Deserializes an object from a string.
		/// </summary>
		/// <param name="type">The type of object to deserialize.</param>
		/// <param name="data">The string data to use for deserialization.</param>
		/// <returns>Returns the deserialized object.</returns>
		object Deserialize(Type type, string data);

		/// <summary>
		/// Deserializes an object.
		/// </summary>
		/// <typeparam name="T">The type of object to deserialize.</typeparam>
		/// <param name="data">The string data to use for deserialization.</param>
		/// <returns>Returns the deserialized object.</returns>
		T Deserialize<T>(string data);

		/// <summary>
		/// Serializes an object to a string.
		/// </summary>
		/// <param name="instance">The object to serialize.</param>
		/// <returns>Returns the object serialized as a string.</returns>
		string Serialize(object instance);
	}
}
