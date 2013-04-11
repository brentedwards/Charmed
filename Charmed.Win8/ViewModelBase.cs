using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Charmed
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(property, value))
			{
				return;
			}

			property = value;
			this.NotifyPropertyChanged(propertyName);
		}

		private bool isBusy;
		public bool IsBusy
		{
			get { return this.isBusy; }
			set
			{
				this.SetProperty(ref this.isBusy, value);
			}
		}
	}
}
