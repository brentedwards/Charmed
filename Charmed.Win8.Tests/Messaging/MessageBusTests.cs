using Charmed.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;

namespace Charmed.Tests.Messaging
{
	[TestClass]
	public class MessageBusTests
	{
		private List<Object> Messages { get; set; }
		private void Handler(Object message)
		{
			Messages.Add(message);
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Messages = new List<Object>();
		}

		[TestMethod]
		public void SubscribeNotExisting()
		{
			// Arrange
			var bus = new MessageBus();
			bus.Subscribe<Object>(Handler);

			// Act
			var message = new Object();
			bus.Publish<Object>(message);

			// Assert
			Assert.AreEqual(1, Messages.Count);
			Assert.AreSame(message, Messages[0]);
		}

		[TestMethod]
		public void SubscribeExisting()
		{
			// Assert
			var bus = new MessageBus();
			bus.Subscribe<Object>(Handler);
			bus.Subscribe<Object>(Handler);

			// Act
			var message = new Object();
			bus.Publish<Object>(message);

			// Assert
			Assert.AreEqual(2, Messages.Count);
			Assert.AreSame(message, Messages[0]);
			Assert.AreSame(message, Messages[1]);
		}

		[TestMethod]
		public void Unsubscribe()
		{
			// Arrange
			var bus = new MessageBus();
			bus.Subscribe<Object>(Handler);

			// Act
			bus.Unsubscribe<Object>(Handler);

			var message = new Object();
			bus.Publish<Object>(message);

			// Assert
			Assert.AreEqual(0, Messages.Count);
		}

		private class TestSubscriber
		{
			public List<Object> Messages { get; set; }

			public TestSubscriber()
			{
				Messages = new List<Object>();
			}

			public void Handler(Object message)
			{
				Messages.Add(message);
			}
		}

		[TestMethod]
		public void Unsubscribe_MultipleInstances_SameClass()
		{
			// Arrange
			var bus = new MessageBus();

			// Subscribe two objects of the same type.
			var subscriber1 = new TestSubscriber();
			bus.Subscribe<Object>(subscriber1.Handler);

			var subscriber2 = new TestSubscriber();
			bus.Subscribe<Object>(subscriber2.Handler);

			// Act

			// Unsubscribe the second of the objects and make sure the other first one still gets messages.
			bus.Unsubscribe<Object>(subscriber2.Handler);

			var message = new Object();
			bus.Publish<Object>(message);

			// Assert
			Assert.AreEqual(0, subscriber2.Messages.Count);
			Assert.AreEqual(1, subscriber1.Messages.Count);
		}

		[TestMethod]
		public void Publish()
		{
			// Arrange
			var bus = new MessageBus();
			bus.Subscribe<Object>(Handler);

			// Act
			var message = Guid.NewGuid().ToString();
			bus.Publish<Object>(message);

			// Assert
			Assert.AreEqual(1, Messages.Count);
		}

		[TestMethod]
		public void PublishDifferent()
		{
			// Arrange
			var bus = new MessageBus();
			bus.Subscribe<Object>(Handler);

			// Act
			var message = Guid.NewGuid().ToString();
			bus.Publish<String>(message);

			// Assert
			Assert.AreEqual(0, Messages.Count);
		}

		[TestMethod]
		public void PublishAndUnsubscribe()
		{
			// Arrange
			var bus = new MessageBus();
			Action<Object> handler = null;
			handler = (m) =>
			{
				Messages.Add(m);

				// Unsubscribe during the publish handler.
				bus.Unsubscribe<Object>(handler);
			};
			bus.Subscribe<Object>(handler);
			bus.Subscribe<Object>((m) =>
			{
				Messages.Add(m);
			});

			// Act
			var message = Guid.NewGuid().ToString();
			bus.Publish<Object>(message);
			bus.Publish<Object>(message);

			// Assert
			Assert.AreEqual(3, Messages.Count);
		}
	}
}
