using Charmed.Container;
using Charmed.Helpers;
using Charmed.Messaging;
using Charmed.Sample.Services;

namespace Charmed.Sample.ViewModels
{
	public sealed class ViewModelLocator
	{
		public ViewModelLocator()
		{
			Ioc.Container.Register<MainViewModel>();
			Ioc.Container.Register<ISerializer, Serializer>();
			Ioc.Container.Register<ISettings, Settings>();
			Ioc.Container.RegisterInstance<IMessageBus>(new MessageBus());
			Ioc.Container.RegisterInstance<IContainer>(Ioc.Container);

#if NETFX_CORE
			Ioc.Container.Register<FeedItemViewModel>();
			Ioc.Container.Register<INavigator, Navigator>();

			Ioc.Container.Register<ISecondaryPinner, SecondaryPinner>();

			Ioc.Container.Register<IRssFeedService, RssFeedService>();
			Ioc.Container.Register<ShellViewModel>();
			Ioc.Container.Register<IShareManager, ShareManager>();
			Ioc.Container.Register<ISettingsManager, SettingsManager>();
			Ioc.Container.Register<IStorage, Storage>();
			Ioc.Container.Register<SettingsViewModel>();
#else
			Ioc.Container.Register<IRssFeedService, WP8RssFeedService>();
#endif // NETFX_CORE
		}

		public MainViewModel Main
		{
			get { return Ioc.Container.Resolve<MainViewModel>();}
		}

#if NETFX_CORE
		public FeedItemViewModel FeedItem
		{
			get { return Ioc.Container.Resolve<FeedItemViewModel>(); }
		}

		public SettingsViewModel Settings
		{
			get { return Ioc.Container.Resolve<SettingsViewModel>(); }
		}
#endif // NETFX_CORE

		public static bool IsInDesignMode
		{
			get
			{
#if NETFX_CORE
				return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#else
				return System.ComponentModel.DesignerProperties.IsInDesignTool;
#endif // NETFX_CORE
			}
		}
	}
}
