using Charmed.Sample.Models;
using Charmed.Sample.Tests.Mocks;
using Charmed.Sample.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;

namespace Charmed.Sample.Win8.Tests.ViewModels
{
	[TestClass]
	public class MainViewModelTests
	{
		private RssFeedServiceMock RssFeedService { get; set; }
		private NavigatorMock Navigator { get; set; }

		private MainViewModel GetViewModel()
		{
			return new MainViewModel(this.RssFeedService, this.Navigator);
		}

		[TestInitialize]
		public void Init()
		{
			this.RssFeedService = new RssFeedServiceMock();
			this.Navigator = new NavigatorMock();
		}

		[TestMethod]
		public void ViewFeed()
		{
			// Arrange
			var viewModel = this.GetViewModel();

			var expectedFeedItem = new FeedItem();

			Type actualViewModelType = null;
			FeedItem actualFeedItem = null;
			this.Navigator.NavigateToViewModelDelegate = (viewModelType, parameter) =>
				{
					actualViewModelType = viewModelType;
					actualFeedItem = parameter as FeedItem;
				};

			// Act
			viewModel.ViewFeed(expectedFeedItem);

			// Assert
			Assert.AreSame(expectedFeedItem, actualFeedItem, "FeedItem");
			Assert.AreEqual(typeof(FeedItemViewModel), actualViewModelType, "ViewModel Type");
		}

		[TestMethod]
		public void FeedData()
		{
			// Arrange
			var viewModel = GetViewModel();

			var expectedFeedData = new List<FeedData>();

			this.RssFeedService.GetFeedsAsyncDelegate = () =>
				{
					return expectedFeedData;
				};

			// Act
			var actualFeedData = viewModel.FeedData;

			// Assert
			Assert.AreSame(expectedFeedData, actualFeedData);
		}
	}
}
