using System;

namespace Charmed
{
	/// <summary>
	/// Interface for navigation in an application.
	/// </summary>
	public interface INavigator
	{
		/// <summary>
		/// Gets whether there is a previous page to go back to.
		/// </summary>
		bool CanGoBack { get; }

		/// <summary>
		/// Goes back to the previous page.
		/// </summary>
		void GoBack();

		/// <summary>
		/// Navigates to the page for a given view model.
		/// </summary>
		/// <typeparam name="TViewModel">The type of view model to navigate to.</typeparam>
		/// <param name="parameter">An optional navigation parameter.</param>
		void NavigateToViewModel<TViewModel>(object parameter = null);

#if WINDOWS_PHONE
		/// <summary>
		/// Removes the back entry.
		/// </summary>
		void RemoveBackEntry();
#endif // WINDOWS_PHONE
	}
}
