using Charmed.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Sample.Tests.Mocks
{
	public class MessageBusMock : IMessageBus
	{
		public Action<object> SubscribeDelegate { get; set; }
		public void Subscribe<TMessage>(Action<TMessage> handler)
		{
			if (SubscribeDelegate != null)
			{
				SubscribeDelegate(handler);
			}
		}

		public Action<object> UnsubscribeDelegate { get; set; }
		public void Unsubscribe<TMessage>(Action<TMessage> handler)
		{
			if (UnsubscribeDelegate != null)
			{
				UnsubscribeDelegate(handler);
			}
		}

		public Action<object> PublishDelegate { get; set; }
		public void Publish<TMessage>(TMessage message)
		{
			if (PublishDelegate != null)
			{
				PublishDelegate(message);
			}
		}
	}
}
