//using Callisto.Controls;
using Charmed.Sample.Models;
using System.Collections.Generic;
//using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;

namespace Charmed.Sample.ViewModels
{
	public sealed class ShellViewModel : ViewModelBase
	{
		//private readonly ISettingsManager settingsManager;
		private readonly ISettings settings;

		public ShellViewModel(
			//ISettingsManager settingsManager,
			ISettings settings)
		{
			//this.settingsManager = settingsManager;
			this.settings = settings;
		}

		public void Initialize()
		{
			//this.settingsManager.Initialize();
			//this.settingsManager.OnSettingsRequested = OnSettingsRequested;

			if (!this.settings.ContainsKey(Constants.FeedsKey))
			{
				// Seed the app with default feeds.
				var feeds = new string[]
				{
					"http://blogs.windows.com/windows/b/appbuilder/rss.aspx",
					"http://blogs.msdn.com/b/b8/rss.aspx"
				};

				this.settings.AddOrUpdate(Constants.FeedsKey, feeds);
			}
		}

		public void Cleanup()
		{
			//this.settingsManager.Cleanup();
		}

		//private void OnSettingsRequested(IList<SettingsCommand> commands)
		//{
		//	SettingsCommand settingsCommand = new SettingsCommand("FeedsSetting", "Feeds", (x) =>
		//	{
		//		SettingsFlyout settings = new SettingsFlyout();
		//		settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;
		//		settings.HeaderText = "Feeds";

		//		var view = new SettingsView();
		//		settings.Content = view;
		//		settings.HorizontalContentAlignment = HorizontalAlignment.Stretch;
		//		settings.VerticalContentAlignment = VerticalAlignment.Stretch;
		//		settings.IsOpen = true;
		//	});
		//	commands.Add(settingsCommand);
		//}
	}
}
