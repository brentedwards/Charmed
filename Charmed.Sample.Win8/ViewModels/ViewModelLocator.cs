using Charmed.Sample.Services;

namespace Charmed.Sample.ViewModels
{
	public sealed class ViewModelLocator
	{
		public MainViewModel Main
		{
			get
			{
				return new MainViewModel(new RssFeedService());
			}
		}
	}
}
