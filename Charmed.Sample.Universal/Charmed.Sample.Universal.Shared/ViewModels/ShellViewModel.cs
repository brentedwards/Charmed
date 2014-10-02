using Charmed.Sample.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Charmed.Sample.ViewModels
{
	public class ShellViewModel : ViewModelBase
	{
		private readonly ISettings settings;

		public ShellViewModel(
			ISettings settings)
		{
			this.settings = settings;
		}

		public virtual void Initialize()
		{
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

		public virtual void Cleanup()
		{
		}
	}
}
