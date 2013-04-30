using Charmed.Sample.Models;
using Charmed.Sample.Tests.Mocks;
using Charmed.Sample.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using Windows.UI.ApplicationSettings;

namespace Charmed.Sample.Win8.Tests.ViewModels
{
	[TestClass]
	public class ShellViewModelTests
	{
		private SettingsManagerMock SettingsManager { get; set; }
		private SettingsMock Settings { get; set; }

		private ShellViewModel GetViewModel()
		{
			return new ShellViewModel(this.SettingsManager, this.Settings);
		}

		[TestInitialize]
		public void Init()
		{
			this.SettingsManager = new SettingsManagerMock();
			this.Settings = new SettingsMock();
		}

		[TestMethod]
		public void Initialize()
		{
			// Arrange
			var viewModel = this.GetViewModel();

			var wasSettingsManagerInitialized = false;
			this.SettingsManager.InitializeDelegate = () =>
				{
					wasSettingsManagerInitialized = true;
				};

			string[] feeds = null;
			this.Settings.AddOrUpdateDelegate = (key, value) =>
				{
					if (key == Constants.FeedsKey)
					{
						feeds = value as string[];
					}
				};

			// Act
			viewModel.Initialize();

			// Assert
			Assert.IsTrue(wasSettingsManagerInitialized, "Was Settings Manager Initialized");
			Assert.IsTrue(feeds.Length > 0, "Has Some Feeds");
		}

		[TestMethod]
		public void Cleanup()
		{
			// Arrange
			var viewModel = this.GetViewModel();

			var wasSettingsManagerCleanedUp = false;
			this.SettingsManager.CleanupDelegate = () =>
				{
					wasSettingsManagerCleanedUp = true;
				};

			// Act
			viewModel.Cleanup();

			// Assert
			Assert.IsTrue(wasSettingsManagerCleanedUp, "Was Settings Manager Cleaned Up");
		}

		[TestMethod]
		public void OnSettingsRequested()
		{
			// Arrange
			var viewModel = this.GetViewModel();

			var commands = new List<SettingsCommand>();

			viewModel.Initialize();

			// Act
			this.SettingsManager.OnSettingsRequested(commands);

			// Assert
			Assert.AreEqual(1, commands.Count);
		}
	}
}
