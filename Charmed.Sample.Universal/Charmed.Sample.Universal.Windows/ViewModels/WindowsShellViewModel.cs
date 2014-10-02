using Charmed.Sample.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;

namespace Charmed.Sample.ViewModels
{
	public sealed class WindowsShellViewModel : ShellViewModel
	{
		private readonly ISettingsManager settingsManager;

		public WindowsShellViewModel(
			ISettingsManager settingsManager,
			ISettings settings)
			: base(settings)
		{
			this.settingsManager = settingsManager;
		}

		public override void Initialize()
		{
			base.Initialize();

			this.settingsManager.Initialize();
			this.settingsManager.OnSettingsRequested = OnSettingsRequested;
		}

		public override void Cleanup()
		{
			this.settingsManager.Cleanup();
		}

		private void OnSettingsRequested(IList<SettingsCommand> commands)
		{
			SettingsCommand settingsCommand = new SettingsCommand("FeedsSetting", "Feeds", (x) =>
			{
				var view = new SettingsView();
				view.Show();
			});
			commands.Add(settingsCommand);
		}
	}
}
