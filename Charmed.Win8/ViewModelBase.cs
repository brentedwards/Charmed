using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Charmed
{
	/// <summary>
	/// A simple base class for view models.
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies that a property has changed.
		/// </summary>
		/// <param name="propertyName">The name of the property that changed.</param>
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Sets a property to a value and notifies that it changed.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="property">The property reference to set.</param>
		/// <param name="data">The data to set.</param>
		/// <param name="propertyName">The name of the property to notify.</param>
		protected void SetProperty<T>(ref T property, T data, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(property, data))
			{
				return;
			}

			property = data;
			this.NotifyPropertyChanged(propertyName);
		}

		private bool isBusy;

		/// <summary>
		/// Gets or sets whether the view model is busy.
		/// </summary>
		public bool IsBusy
		{
			get { return this.isBusy; }
			set { this.SetProperty(ref this.isBusy, value); }
		}
	}
}
