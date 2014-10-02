using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;

namespace Charmed
{
	public sealed class SettingsManager : ISettingsManager
	{
		private SettingsPane SettingsPane { get; set; }

		public void Initialize()
		{
			this.SettingsPane = SettingsPane.GetForCurrentView();
			this.SettingsPane.CommandsRequested += SettingsPane_CommandsRequested;
		}

		public void Cleanup()
		{
			this.SettingsPane.CommandsRequested -= SettingsPane_CommandsRequested;
		}

		private void SettingsPane_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
		{
			if (this.OnSettingsRequested != null)
			{
				this.OnSettingsRequested(args.Request.ApplicationCommands);
			}
		}

		public Action<IList<SettingsCommand>> OnSettingsRequested { get; set; }
	}
}
