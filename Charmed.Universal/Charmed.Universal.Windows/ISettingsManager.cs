using System;
using System.Collections.Generic;
using Windows.UI.ApplicationSettings;

namespace Charmed
{
	/// <summary>
	/// Interface for managing interactions with the settings charm.
	/// </summary>
	public interface ISettingsManager
	{
		/// <summary>
		/// Initializes the Settings Manager.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Cleans up the Settings Manager.
		/// </summary>
		void Cleanup();

		/// <summary>
		/// Action which is called when the user has requested the settings.
		/// </summary>
		Action<IList<SettingsCommand>> OnSettingsRequested { get; set; }
	}
}
