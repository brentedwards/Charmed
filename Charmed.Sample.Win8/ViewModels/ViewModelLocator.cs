using Charmed.Container;
using Charmed.Sample.Services;

namespace Charmed.Sample.ViewModels
{
	public sealed class ViewModelLocator
	{
		public MainViewModel Main
		{
			get
			{
				return Ioc.Container.Resolve<MainViewModel>();
			}
		}

		public FeedItemViewModel FeedItem
		{
			get
			{
				return Ioc.Container.Resolve<FeedItemViewModel>();
			}
		}
	}
}
