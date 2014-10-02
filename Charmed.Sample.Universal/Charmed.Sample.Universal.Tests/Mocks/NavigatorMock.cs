using System;

namespace Charmed.Sample.Tests.Mocks
{
	public class NavigatorMock : INavigator
	{
		public bool CanGoBack { get; set; }

		public Action GoBackDelegate { get; set; }
		public void GoBack()
		{
			if (this.GoBackDelegate != null)
			{
				this.GoBackDelegate();
			}
		}

		public Action<Type, object> NavigateToViewModelDelegate { get; set; }
		public void NavigateToViewModel<TViewModel>(object parameter = null)
		{
			if (this.NavigateToViewModelDelegate != null)
			{
				this.NavigateToViewModelDelegate(typeof(TViewModel), parameter);
			}
		}

#if WINDOWS_PHONE
		public Action RemoveBackEntryDelegate { get; set; }
		public void RemoveBackEntry()
		{
			if (this.RemoveBackEntryDelegate != null)
			{
				this.RemoveBackEntryDelegate();
			}
		}
#endif // WINDOWS_PHONE
	}
}
