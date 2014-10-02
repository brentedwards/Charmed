using Charmed.Sample.Models;
using Charmed.Sample.Tests.Mocks;
using Charmed.Sample.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;

namespace Charmed.Sample.Tests.ViewModels
{
	[TestClass]
	public sealed class FeedItemViewModelTests
	{
		private ShareManagerMock ShareManager { get; set; }
		private SerializerMock Serializer { get; set; }
		private SecondaryPinnerMock SecondaryPinner { get; set; }
		private StorageMock Storage { get; set; }

		private WindowsFeedItemViewModel GetViewModel()
		{
			return new WindowsFeedItemViewModel(this.Serializer, this.Storage, this.SecondaryPinner, this.ShareManager);
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
			this.SecondaryPinner = new SecondaryPinnerMock();
			this.Storage = new StorageMock();

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

		[TestMethod]
		public async Task Pin_PinSucceeded()
		{
			// Arrange
			var viewModel = GetViewModel();

			var feedItem = new FeedItem
			{
				Title = Guid.NewGuid().ToString(),
				Author = Guid.NewGuid().ToString(),
				Link = new Uri("http://www.bing.com")
			};

			viewModel.LoadState(feedItem, null);

			Placement actualPlacement = Placement.Default;
			TileInfo actualTileInfo = null;
			SecondaryPinner.PinDelegate = (tileInfo) =>
			{
				actualPlacement = tileInfo.RequestPlacement;
				actualTileInfo = tileInfo;

				return true;
			};

			string actualKey = null;
			List<FeedItem> actualPinnedFeedItems = null;
			Storage.SaveAsyncDelegate = (key, value) =>
			{
				actualKey = key;
				actualPinnedFeedItems = (List<FeedItem>)value;
			};

			// Act
			await viewModel.Pin(null);

			// Assert
			Assert.AreEqual(Placement.Above, actualPlacement, "Placement");
			Assert.AreEqual(string.Format(Constants.SecondaryIdFormat, viewModel.FeedItem.Id), actualTileInfo.TileId, "Tile Info Tile Id");
			Assert.AreEqual(viewModel.FeedItem.Title, actualTileInfo.DisplayName, "Tile Info Display Name");
			Assert.AreEqual(viewModel.FeedItem.Id.ToString(), actualTileInfo.Arguments, "Tile Info Arguments");
			Assert.AreEqual(Constants.PinnedFeedItemsKey, actualKey, "Save Key");
			Assert.IsNotNull(actualPinnedFeedItems, "Pinned Feed Items");
		}

		[TestMethod]
		public async Task Pin_PinNotSucceeded()
		{
			// Arrange
			var viewModel = GetViewModel();

			var feedItem = new FeedItem
			{
				Title = Guid.NewGuid().ToString(),
				Author = Guid.NewGuid().ToString(),
				Link = new Uri("http://www.bing.com")
			};

			viewModel.LoadState(feedItem, null);

			Placement actualPlacement = Placement.Default;
			TileInfo actualTileInfo = null;
			SecondaryPinner.PinDelegate = (tileInfo) =>
			{
				actualPlacement = tileInfo.RequestPlacement;
				actualTileInfo = tileInfo;

				return false;
			};

			var wasSaveCalled = false;
			Storage.SaveAsyncDelegate = (key, value) =>
			{
				wasSaveCalled = true;
			};

			// Act
			await viewModel.Pin(null);

			// Assert
			Assert.AreEqual(Placement.Above, actualPlacement, "Placement");
			Assert.AreEqual(string.Format(Constants.SecondaryIdFormat, viewModel.FeedItem.Id), actualTileInfo.TileId, "Tile Info Tile Id");
			Assert.AreEqual(viewModel.FeedItem.Title, actualTileInfo.DisplayName, "Tile Info Display Name");
			Assert.AreEqual(viewModel.FeedItem.Id.ToString(), actualTileInfo.Arguments, "Tile Info Arguments");
			Assert.IsFalse(wasSaveCalled, "Was Save Called");
		}
	}
}
