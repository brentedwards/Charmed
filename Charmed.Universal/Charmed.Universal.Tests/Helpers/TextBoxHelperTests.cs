using Charmed.Helpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using System;
using Windows.UI.Xaml.Controls;

namespace Charmed.Tests.Helpers
{
	[TestClass]
	public class TextBoxHelperTests
	{
		[UITestMethod]
		public void BindableText()
		{
			// Arrange
			var textBox = new TextBox();

			var expectedText = Guid.NewGuid().ToString();

			// Act
			TextBoxHelper.SetBindableText(textBox, expectedText);

			// Assert
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(expectedText, textBox.Text);
		}
	}
}
