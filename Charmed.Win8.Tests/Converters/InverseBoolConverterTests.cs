using Charmed.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Charmed.Tests.Converters
{
	[TestClass]
	public class InverseBoolConverterTests
	{
		[TestMethod]
		public void Convert_True()
		{
			// Arrange
			var converter = new InverseBoolConverter();

			// Act
			var result = (bool)converter.Convert(true, null, null, null);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Convert_False()
		{
			// Arrange
			var converter = new InverseBoolConverter();

			// Act
			var result = (bool)converter.Convert(false, null, null, null);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Convert_NotBool()
		{
			// Arrange
			var converter = new InverseBoolConverter();

			// Act
			var result = (bool)converter.Convert(new object(), null, null, null);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void ConvertBack_True()
		{
			// Arrange
			var converter = new InverseBoolConverter();

			// Act
			var result = (bool)converter.ConvertBack(true, null, null, null);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void ConvertBack_False()
		{
			// Arrange
			var converter = new InverseBoolConverter();

			// Act
			var result = (bool)converter.ConvertBack(false, null, null, null);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void ConvertBack_NotBool()
		{
			// Arrange
			var converter = new InverseBoolConverter();

			// Act
			var result = (bool)converter.ConvertBack(new object(), null, null, null);

			// Assert
			Assert.IsFalse(result);
		}
	}
}
