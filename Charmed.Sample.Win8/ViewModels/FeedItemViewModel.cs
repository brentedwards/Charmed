using Charmed.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Sample.ViewModels
{
	public sealed class FeedItemViewModel : ViewModelBase
	{
		private FeedItem navigationParameter;
		public FeedItem NavigationParameter
		{
			get { return this.navigationParameter; }
			set
			{
				this.navigationParameter = value;
				NotifyPropertyChanged("NavigationParameter");
			}
		}
	}
}
