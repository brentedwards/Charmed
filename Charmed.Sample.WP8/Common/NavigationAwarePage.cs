using Microsoft.Phone.Controls;

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
