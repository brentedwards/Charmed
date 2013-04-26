using Charmed.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Windows.UI.Xaml;

namespace Charmed.Tests.Converters
{
	[TestClass]
	public class BoolToVisibilityConverterTests
	{
		[TestMethod]
		public void Convert_True()
		{
			// Arrange
			var converter = new BoolToVisibilityConverter();

			// Act
			var result = (Visibility)converter.Convert(true, null, null, null);

			// Assert
			Assert.AreEqual(Visibility.Visible, result);
		}

		[TestMethod]
		public void Convert_False()
		{
			// Arrange
			var converter = new BoolToVisibilityConverter();

			// Act
			var result = (Visibility)converter.Convert(false, null, null, null);

			// Assert
			Assert.AreEqual(Visibility.Collapsed, result);
		}

		[TestMethod]
		public void Convert_NotBool()
		{
			// Arrange
			var converter = new BoolToVisibilityConverter();

			// Act
			var result = (Visibility)converter.Convert(new object(), null, null, null);

			// Assert
			Assert.AreEqual(Visibility.Collapsed, result);
		}

		[TestMethod]
		public void ConvertBack()
		{
			// Arrange
			var converter = new BoolToVisibilityConverter();

			// Act

			// Assert
			Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(null, null, null, null));
		}
	}
}
