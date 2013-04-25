using Charmed.Sample.Messages;
using Charmed.Sample.Tests.Mocks;
using Charmed.Sample.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Charmed.Sample.Win8.Tests.ViewModels
{
	[TestClass]
	public class SettingsViewModelTests
	{
		private SettingsMock Settings { get; set; }
		private MessageBusMock MessageBox { get; set; }

		private SettingsViewModel GetViewModel()
		{
			return new SettingsViewModel(this.Settings, this.MessageBox);
		}

		private List<string> PropertiesChanged { get; set; }
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			PropertiesChanged.Add(args.PropertyName);
		}

		[TestInitialize]
		public void Init()
		{
			this.Settings = new SettingsMock();
			this.MessageBox = new MessageBusMock();

			this.PropertiesChanged = new List<string>();
		}

		[TestMethod]
		public void InitiallyLoadFeeds()
		{
			// Arrange
			var feeds = new string[] { Guid.NewGuid().ToString() };

			this.Settings.TryGetValueDelegate = (key, value) =>
				{
					return feeds;
				};

			// Act
			var viewModel = GetViewModel();

			// Assert
			Assert.AreEqual(feeds[0], viewModel.Feeds[0]);
		}

		[TestMethod]
		public void NewFeed()
		{
			// Arrange
			var viewModel = GetViewModel();

			var expectedNewFeed = Guid.NewGuid().ToString();

			// Act
			viewModel.PropertyChanged += OnPropertyChanged;
			viewModel.NewFeed = expectedNewFeed;
			viewModel.PropertyChanged -= OnPropertyChanged;

			// Assert
			Assert.AreEqual(expectedNewFeed, viewModel.NewFeed, "NewFeed");
			Assert.IsTrue(this.PropertiesChanged.Contains("NewFeed"), "Property Changed");
		}

		[TestMethod]
		public void AddFeed()
		{
			// Arrange
			var viewModel = GetViewModel();

			var expectedNewFeed = Guid.NewGuid().ToString();
			viewModel.NewFeed = expectedNewFeed;

			var wasSettingsUpdated = false;
			this.Settings.AddOrUpdateDelegate = (key, value) =>
				{
					wasSettingsUpdated = true;
				};

			var wasMessagePublished = false;
			this.MessageBox.PublishDelegate = (m) =>
				{
					wasMessagePublished = m is FeedsChangedMessage;
				};

			// Act
			viewModel.AddFeed();

			// Assert
			Assert.IsTrue(viewModel.Feeds.Contains(expectedNewFeed), "Feeds");
			Assert.AreEqual(string.Empty, viewModel.NewFeed, "NewFeed");
			Assert.IsTrue(wasSettingsUpdated, "Was Settings Updated");
			Assert.IsTrue(wasMessagePublished, "Was Message Published");
		}

		[TestMethod]
		public void RemoveFeed()
		{
			// Arrange
			var viewModel = GetViewModel();

			var expectedNewFeed = Guid.NewGuid().ToString();
			viewModel.NewFeed = expectedNewFeed;
			viewModel.AddFeed();

			var wasSettingsUpdated = false;
			this.Settings.AddOrUpdateDelegate = (key, value) =>
			{
				wasSettingsUpdated = true;
			};

			var wasMessagePublished = false;
			this.MessageBox.PublishDelegate = (m) =>
			{
				wasMessagePublished = m is FeedsChangedMessage;
			};

			// Act
			viewModel.RemoveFeed(expectedNewFeed);

			// Assert
			Assert.IsFalse(viewModel.Feeds.Contains(expectedNewFeed), "Feeds");
			Assert.AreEqual(string.Empty, viewModel.NewFeed, "NewFeed");
			Assert.IsTrue(wasSettingsUpdated, "Was Settings Updated");
			Assert.IsTrue(wasMessagePublished, "Was Message Published");
		}
	}
}
