using System;
using System.Collections.Generic;
using Windows.UI.ApplicationSettings;

namespace Charmed.Sample.Tests.Mocks
{
	public class SettingsManagerMock : ISettingsManager
	{
		public Action InitializeDelegate { get; set; }
		public void Initialize()
		{
			if (this.InitializeDelegate != null)
			{
				this.InitializeDelegate();
			}
		}

		public Action CleanupDelegate { get; set; }
		public void Cleanup()
		{
			if (this.CleanupDelegate != null)
			{
				this.CleanupDelegate();
			}
		}

		public Action<IList<SettingsCommand>> OnSettingsRequested { get; set; }
	}
}
