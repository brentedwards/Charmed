using System;

namespace Charmed.Messaging
{
	/// <summary>
	///	Maintains a weak reference to an action/delegate.
	/// </summary>
	public sealed class ActionReference
	{
		private WeakReference WeakReference { get; set; }

		/// <summary>
		/// Gets the action/delegate this reference targets.
		/// </summary>
		public Delegate Target { get; private set; }

		/// <summary>
		/// Gets whether the reference is still alive.
		/// </summary>
		public bool IsAlive
		{
			get { return WeakReference.IsAlive; }
		}

		/// <summary>
		/// Constructor for ActionReference.
		/// </summary>
		/// <param name="action">The action/delegate to be referenced.</param>
		public ActionReference(Delegate action)
		{
			Target = action;
			WeakReference = new WeakReference(action.Target);
		}
	}
}
