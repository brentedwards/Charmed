using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Sample.Common
{
	public abstract class NavigationAwarePage : PhoneApplicationPage
	{
		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			var viewModel = this.DataContext as ViewModelBase;
			if (viewModel != null)
			{
				string serializedParameter;
				if (!NavigationContext.QueryString.TryGetValue("parameter", out serializedParameter))
				{
					serializedParameter = null;
				}

				viewModel.LoadState(serializedParameter, null);
			}
		}

		protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
		{
			var viewModel = this.DataContext as ViewModelBase;
			if (viewModel != null)
			{
				viewModel.SaveState(null);
			}
		}
	}
}
