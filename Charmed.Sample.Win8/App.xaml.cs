using Charmed.Container;
using Charmed.Sample.Models;
using Charmed.Sample.Services;
using Charmed.Sample.ViewModels;
using Charmed.Sample.Views;
using Charmed.Sample.Win8.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace Charmed.Sample.Win8
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            
            if (rootFrame == null)
            {
				if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.FeedsKey))
				{
					// Seed the app with default feeds.
					var feeds = new string[]
					{
						"http://blogs.windows.com/windows/b/windowsexperience/atom.aspx",
						"http://blogs.windows.com/windows/b/extremewindows/atom.aspx",
						"http://blogs.windows.com/windows/b/bloggingwindows/atom.aspx",
						"http://blogs.windows.com/windows_live/b/windowslive/rss.aspx",
						"http://blogs.windows.com/windows_live/b/developer/atom.aspx",
						"http://blogs.windows.com/windows_phone/b/wpdev/atom.aspx",
						"http://blogs.windows.com/windows_phone/b/wmdev/atom.aspx",
						"http://blogs.windows.com/windows_phone/b/windowsphone/atom.aspx"
					};

					ApplicationData.Current.RoamingSettings.Values[Constants.FeedsKey] = feeds;
				}

                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
				Ioc.Container.Resolve<INavigator>().NavigateToViewModel<MainViewModel>();
            }

			if (!string.IsNullOrWhiteSpace(args.Arguments))
			{
				var storage = Ioc.Container.Resolve<IStorage>();
				List<FeedItem> pinnedFeedItems = await storage.LoadAsync<List<FeedItem>>(Constants.PinnedFeedItemsKey);
				if (pinnedFeedItems != null)
				{
					int id;
					if (int.TryParse(args.Arguments, out id))
					{
						var pinnedFeedItem = pinnedFeedItems.FirstOrDefault(fi => fi.Id == id);
						if (pinnedFeedItem != null)
						{
							Ioc.Container.Resolve<INavigator>().NavigateToViewModel<FeedItemViewModel>(pinnedFeedItem);
						}
					}
				}
			}

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
