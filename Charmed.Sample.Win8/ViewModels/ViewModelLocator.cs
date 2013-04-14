using Charmed.Container;
using Charmed.Helpers;
using Charmed.Sample.Services;

namespace Charmed.Sample.ViewModels
{
	public sealed class ViewModelLocator
	{
		public ViewModelLocator()
		{
			Ioc.Container.Register<MainViewModel>();
			Ioc.Container.Register<FeedItemViewModel>();
			Ioc.Container.Register<IRssFeedService, RssFeedService>();
			Ioc.Container.Register<ISerializer, Serializer>();
			Ioc.Container.Register<INavigator, Navigator>();
			Ioc.Container.Register<IShareManager, ShareManager>();
			Ioc.Container.RegisterInstance<IContainer>(Ioc.Container);
		}

		public MainViewModel Main
		{
			get { return Ioc.Container.Resolve<MainViewModel>();}
		}

		public FeedItemViewModel FeedItem
		{
			get { return Ioc.Container.Resolve<FeedItemViewModel>(); }
		}

		public static bool IsInDesignMode
		{
			get { return Windows.ApplicationModel.DesignMode.DesignModeEnabled; }
		}
	}
}
