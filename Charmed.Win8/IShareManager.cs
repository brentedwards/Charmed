using System;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed
{
	/// <summary>
	/// Interface for managing interactions with the share charm.
	/// </summary>
	public interface IShareManager
	{
		/// <summary>
		/// Initializes the Share Manager.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Cleans up the Share Manager.
		/// </summary>
		void Cleanup();

		/// <summary>
		/// Action which is called when the user has requested to share.
		/// </summary>
		Action<DataPackage> OnShareRequested { get; set; }
	}
}
