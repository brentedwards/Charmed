using Charmed.Container;
using System;
using System.Collections.Generic;

namespace Charmed.Sample.ViewModels
{
	/// <summary>
	/// Sample view model base class that inherits from the Charmed ViewModelBase class.
	/// This base class adds some support for loading and saving state to integrate with
	/// with LayoutAwarePage.
	/// </summary>
	public abstract class SampleViewModelBase : ViewModelBase
	{
		/// <summary>
		/// Loads state when the view model is loaded.
		/// </summary>
		/// <param name="navigationParameter">The navigation parameter.</param>
		/// <param name="pageState">The state to load.</param>
		public virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
		{
		}

		/// <summary>
		/// Saves state when the view model is unloaded.
		/// </summary>
		/// <param name="pageState">The state to save to.</param>
		public virtual void SaveState(Dictionary<String, Object> pageState)
		{
		}
	}

	/// <summary>
	/// Sample view model base class that provides strongly typed navigation parameters.
	/// </summary>
	/// <typeparam name="TParameter"></typeparam>
	public abstract class SampleViewModelBase<TParameter> : SampleViewModelBase
	{
		/// <summary>
		/// Loads state when the view model is loaded, with a strongly-typed parameter.
		/// </summary>
		/// <param name="navigationParameter">The navigation parameter.</param>
		/// <param name="pageState">The state to load.</param>
		public abstract void LoadState(TParameter navigationParameter, Dictionary<String, Object> pageState);

		/// <summary>
		/// Loads state when the view model is loaded.
		/// </summary>
		/// <param name="navigationParameter">The navigation parameter.</param>
		/// <param name="pageState">The state to load.</param>
		public override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
		{
			var serializer = Ioc.Container.Resolve<ISerializer>();
			var deserializedNavigationParameter = serializer.Deserialize<TParameter>(navigationParameter.ToString());
			this.LoadState(deserializedNavigationParameter, pageState);
		}
	}
}
