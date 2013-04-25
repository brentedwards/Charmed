using System;
using System.Collections.Generic;
using System.Reflection;

namespace Charmed.Messaging
{
	/// <summary>
	/// A Message Bus is a central messaging system for
	/// an application.
	/// </summary>
	public sealed class MessageBus : IMessageBus
	{
		private readonly Dictionary<Type, List<ActionReference>> subscribers =
			new Dictionary<Type, List<ActionReference>>();

		private readonly object @lock = new object();

		/// <summary>
		/// Subscribes an action to a particular message type.  When that message type
		/// is published, this action will be called.
		/// </summary>
		/// <typeparam name="TMessage">The type of message to listen for.</typeparam>
		/// <param name="handler">
		/// The action which will be called when a message of type <typeparamref name="TMessage"/>
		/// is published.
		/// </param>
		public void Subscribe<TMessage>(Action<TMessage> handler)
		{
			lock (this.@lock)
			{
				if (this.subscribers.ContainsKey(typeof(TMessage)))
				{
					var handlers = this.subscribers[typeof(TMessage)];
					handlers.Add(new ActionReference(handler));
				}
				else
				{
					var handlers = new List<ActionReference>();
					handlers.Add(new ActionReference(handler));
					this.subscribers[typeof(TMessage)] = handlers;
				}
			}
		}

		/// <summary>
		/// Unsubscribes an action from a particular message type.
		/// </summary>
		/// <typeparam name="TMessage">The type of message to stop listening for.</typeparam>
		/// <param name="handler">
		/// The action which is to be unsubscribed from messages of type <typeparamref name="TMessage"/>.
		/// </param>
		public void Unsubscribe<TMessage>(Action<TMessage> handler)
		{
			lock (this.@lock)
			{
				if (this.subscribers.ContainsKey(typeof(TMessage)))
				{
					var handlers = this.subscribers[typeof(TMessage)];

					ActionReference targetReference = null;
					foreach (var reference in handlers)
					{
						var action = (Action<TMessage>)reference.Target;
						if ((action.Target == handler.Target) && action.GetMethodInfo().Name.Equals(handler.GetMethodInfo().Name))
						{
							targetReference = reference;
							break;
						}
					}
					handlers.Remove(targetReference);

					if (handlers.Count == 0)
					{
						this.subscribers.Remove(typeof(TMessage));
					}
				}
			}
		}

		/// <summary>
		/// Publishes a message to any subscribers of a particular message type.
		/// </summary>
		/// <typeparam name="TMessage">The type of message to publish.</typeparam>
		/// <param name="message">The message to be published</param>
		public void Publish<TMessage>(TMessage message)
		{
			var subscribers = this.RefreshAndGetSubscribers<TMessage>();
			foreach (var subscriber in subscribers)
			{
				subscriber.Invoke(message);
			}
		}

		private List<Action<TMessage>> RefreshAndGetSubscribers<TMessage>()
		{
			var toCall = new List<Action<TMessage>>();
			var toRemove = new List<ActionReference>();

			lock (this.@lock)
			{
				if (this.subscribers.ContainsKey(typeof(TMessage)))
				{
					var handlers = this.subscribers[typeof(TMessage)];
					foreach (var handler in handlers)
					{
						if (handler.IsAlive)
						{
							toCall.Add((Action<TMessage>)handler.Target);
						}
						else
						{
							toRemove.Add(handler);
						}
					}

					foreach (var remove in toRemove)
					{
						handlers.Remove(remove);
					}

					if (handlers.Count == 0)
					{
						subscribers.Remove(typeof(TMessage));
					}
				}
			}

			return toCall;
		}
	}
}
