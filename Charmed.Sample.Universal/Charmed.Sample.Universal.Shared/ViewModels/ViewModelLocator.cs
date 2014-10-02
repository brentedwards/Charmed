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
			Ioc.Container.Register<IStorage, Storage>();
			Ioc.Container.RegisterInstance<IMessageBus>(new MessageBus());
			Ioc.Container.RegisterInstance<IContainer>(Ioc.Container);
			Ioc.Container.Register<ISecondaryPinner, SecondaryPinner>();
			Ioc.Container.Register<IRssFeedService, RssFeedService>();
			Ioc.Container.Register<INavigator, Navigator>();

#if WINDOWS_APP

			Ioc.Container.Register<ShellViewModel, WindowsShellViewModel>();
			Ioc.Container.Register<IShareManager, ShareManager>();
			Ioc.Container.Register<ISettingsManager, SettingsManager>();
			Ioc.Container.Register<SettingsViewModel>();
			Ioc.Container.Register<FeedItemViewModel, WindowsFeedItemViewModel>();
#else
			Ioc.Container.Register<ShellViewModel>();
			Ioc.Container.Register<FeedItemViewModel>();
#endif // WINDOWS_APP
		}

		public MainViewModel Main
		{
			get { return Ioc.Container.Resolve<MainViewModel>();}
		}

		public FeedItemViewModel FeedItem
		{
			get { return Ioc.Container.Resolve<FeedItemViewModel>(); }
		}

#if NETFX_CORE
		public SettingsViewModel Settings
		{
			get { return Ioc.Container.Resolve<SettingsViewModel>(); }
		}
#else
		public SplashViewModel Splash
		{
			get { return Ioc.Container.Resolve<SplashViewModel>(); }
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
