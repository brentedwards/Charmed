using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	}
}
