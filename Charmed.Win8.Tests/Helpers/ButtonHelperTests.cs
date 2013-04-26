using Charmed.Helpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;

namespace Charmed.Tests.Helpers
{
	[TestClass]
	public class ButtonHelperTests
	{
		[UITestMethod]
		public void Click()
		{
			// Arrange
			var mock = new Mock();
			var button = new Button();
			button.DataContext = mock;

			ButtonHelper.SetMethodName(button, "DoStuff");

			// Act
			var peer = new ButtonAutomationPeer(button);
			var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
			invokeProvider.Invoke();

			// Assert
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(mock.WasButtonClicked);
		}

		[UITestMethod]
		public void Click_WithParameter()
		{
			// Arrange
			var mock = new Mock();
			var button = new Button();
			button.DataContext = mock;

			var expectedParameter = Guid.NewGuid().ToString();

			ButtonHelper.SetMethodName(button, "DoStuffWithParameter");
			ButtonHelper.SetParameter(button, expectedParameter);

			// Act
			var peer = new ButtonAutomationPeer(button);
			var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
			invokeProvider.Invoke();

			// Assert
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(expectedParameter, mock.Parameter, "Parameter");
		}

		[UITestMethod]
		public void Click_WithButtonParameter()
		{
			// Arrange
			var mock = new Mock();
			var button = new Button();
			button.DataContext = mock;

			ButtonHelper.SetMethodName(button, "DoStuffWithButtonParameter");

			// Act
			var peer = new ButtonAutomationPeer(button);
			var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
			invokeProvider.Invoke();

			// Assert
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreSame(button, mock.ButtonParameter, "Parameter");
		}

		[UITestMethod]
		public void Click_WithButtonAndStringParameter()
		{
			// Arrange
			var mock = new Mock();
			var button = new Button();
			button.DataContext = mock;

			var expectedParameter = Guid.NewGuid().ToString();

			ButtonHelper.SetMethodName(button, "DoStuffWithButtonAndStringParameter");
			ButtonHelper.SetParameter(button, expectedParameter);

			// Act
			var peer = new ButtonAutomationPeer(button);
			var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
			invokeProvider.Invoke();

			// Assert
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreSame(button, mock.ButtonParameter, "Button Parameter");
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(expectedParameter, mock.Parameter, "Parameter");
		}

		[UITestMethod]
		public void Click_SetContext_WithButtonAndStringParameter()
		{
			// Arrange
			var mock = new Mock();
			var button = new Button();

			var expectedParameter = Guid.NewGuid().ToString();

			ButtonHelper.SetMethodName(button, "DoStuffWithButtonAndStringParameter");
			ButtonHelper.SetParameter(button, expectedParameter);
			ButtonHelper.SetMethodContext(button, mock);

			// Act
			var peer = new ButtonAutomationPeer(button);
			var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
			invokeProvider.Invoke();

			// Assert
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreSame(button, mock.ButtonParameter, "Button Parameter");
			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(expectedParameter, mock.Parameter, "Parameter");
		}

		public class Mock
		{
			public bool WasButtonClicked { get; set; }
			public void DoStuff()
			{
				this.WasButtonClicked = true;
			}

			public string Parameter { get; set; }
			public void DoStuffWithParameter(string parameter)
			{
				this.Parameter = parameter;
			}

			public Button ButtonParameter { get; set; }
			public void DoStuffWithButtonParameter(Button buttonParameter)
			{
				this.ButtonParameter = buttonParameter;
			}

			public void DoStuffWithButtonAndStringParameter(Button buttonParameter, string parameter)
			{
				this.Parameter = parameter;
				this.ButtonParameter = buttonParameter;
			}
		}
	}
}
