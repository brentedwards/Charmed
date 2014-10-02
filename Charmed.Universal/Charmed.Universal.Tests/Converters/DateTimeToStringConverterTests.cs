using Charmed.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace Charmed.Tests.Converters
{
	[TestClass]
	public class DateTimeToStringConverterTests
	{
		[TestMethod]
		public void Convert()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();

			var date = DateTime.Now;

			// Act
			var result = (string)converter.Convert(date, null, null, null);

			// Assert
			Assert.AreEqual(date.ToString("d"), result);
		}

		[TestMethod]
		public void Convert_CustomFormat()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();
			converter.Format = "dd";

			var date = DateTime.Now;

			// Act
			var result = (string)converter.Convert(date, null, null, null);

			// Assert
			Assert.AreEqual(date.ToString("dd"), result);
		}

		[TestMethod]
		public void Convert_Null()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();

			// Act
			var result = (string)converter.Convert(null, null, null, null);

			// Assert
			Assert.AreEqual(string.Empty, result);
		}

		[TestMethod]
		public void Convert_NotDateTime()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();

			// Act
			var result = (string)converter.Convert(new object(), null, null, null);

			// Assert
			Assert.AreEqual(string.Empty, result);
		}

		[TestMethod]
		public void ConvertBack()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();

			var date = DateTime.Today;

			// Act
			var result = (DateTime)converter.ConvertBack(date.ToString("d"), null, null, null);

			// Assert
			Assert.AreEqual(date, result);
		}

		[TestMethod]
		public void ConvertBack_Null_NotNullable()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();

			// Act
			var result = (DateTime)converter.ConvertBack(null, null, null, null);

			// Assert
			Assert.AreEqual(new DateTime(), result);
		}

		[TestMethod]
		public void ConvertBack_Null_Nullable()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();
			converter.IsDateTimeNullable = true;

			// Act
			var result = converter.ConvertBack(null, null, null, null);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void ConvertBack_NotParsable_NotNullable()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();

			// Act
			var result = (DateTime)converter.ConvertBack(Guid.NewGuid().ToString(), null, null, null);

			// Assert
			Assert.AreEqual(new DateTime(), result);
		}

		[TestMethod]
		public void ConvertBack_NotParsable_Nullable()
		{
			// Arrange
			var converter = new DateTimeToStringConverter();
			converter.IsDateTimeNullable = true;

			// Act
			var result = converter.ConvertBack(Guid.NewGuid().ToString(), null, null, null);

			// Assert
			Assert.IsNull(result);
		}
	}
}
