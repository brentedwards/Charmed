using Charmed.Sample.Models;
using Charmed.Sample.Tests.Mocks;
using Charmed.Sample.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed.Sample.Win8.Tests.ViewModels
{
	[TestClass]
	public class FeedItemViewModelTests
	{
		private ShareManagerMock ShareManager { get; set; }
		private SerializerMock Serializer { get; set; }

		private FeedItemViewModel GetViewModel()
		{
			return new FeedItemViewModel(this.ShareManager, this.Serializer);
		}

		private List<string> PropertiesChanged { get; set; }
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			this.PropertiesChanged.Add(args.PropertyName);
		}

		[TestInitialize]
		public void Init()
		{
			this.ShareManager = new ShareManagerMock();
			this.Serializer = new SerializerMock();

			this.PropertiesChanged = new List<string>();
		}

		[TestMethod]
		public void LoadState()
		{
			// Arrange
			var viewModel = GetViewModel();

			var expectedFeedItem = new FeedItem();

			var wasInitializeCalled = false;
			this.ShareManager.InitializeDelegate = () =>
				{
					wasInitializeCalled = true;
				};

			// Act
			viewModel.LoadState(expectedFeedItem, null);

			// Assert
			Assert.AreSame(expectedFeedItem, viewModel.FeedItem, "FeedItem");
			Assert.IsTrue(wasInitializeCalled, "Share Manager Initialize");
		}

		[TestMethod]
		public void SaveState()
		{
			// Arrange
			var viewModel = GetViewModel();

			var expectedFeedItem = new FeedItem();

			var wasCleanupCalled = false;
			this.ShareManager.CleanupDelegate = () =>
			{
				wasCleanupCalled = true;
			};

			// Act
			viewModel.SaveState(null);

			// Assert
			Assert.IsTrue(wasCleanupCalled, "Share Manager Initialize");
		}

		[TestMethod]
		public void FeedItem()
		{
			// Arrange
			var viewModel = GetViewModel();

			var expectedFeedItem = new FeedItem();

			// Act
			viewModel.PropertyChanged += this.OnPropertyChanged;
			viewModel.FeedItem = expectedFeedItem;
			viewModel.PropertyChanged -= this.OnPropertyChanged;

			// Assert
			Assert.IsTrue(this.PropertiesChanged.Contains("FeedItem"), "Property Changed");
			Assert.AreSame(expectedFeedItem, viewModel.FeedItem, "Feed Item");
		}

		[TestMethod]
		public async Task ShareRequested()
		{
			// Arrange
			var viewModel = GetViewModel();

			var dataPackage = new DataPackage();
			var dataPackageView = dataPackage.GetView();

			var feedItem = new FeedItem
			{
				Title = Guid.NewGuid().ToString(),
				Author = Guid.NewGuid().ToString(),
				Link = new Uri("http://www.bing.com")
			};

			viewModel.LoadState(feedItem, null);

			// Act
			this.ShareManager.OnShareRequested(dataPackage);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(await dataPackageView.GetTextAsync()), "Text Exists");
			Assert.AreEqual(feedItem.Link.ToString(), (await dataPackageView.GetUriAsync()).ToString(), "Uri");
			Assert.IsTrue(!string.IsNullOrEmpty(await dataPackageView.GetHtmlFormatAsync()), "HTML Exists");
		}
	}
}
