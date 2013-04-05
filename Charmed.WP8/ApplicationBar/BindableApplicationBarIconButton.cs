using Microsoft.Phone.Shell;
using System;

namespace Charmed.ApplicationBar
{
	/// <summary>
	/// Application Bar Icon Button that allows for routing the click to a method on the view model.
	/// </summary>
	public sealed class BindableApplicationBarIconButton : ApplicationBarIconButton
	{
		public BindableApplicationBarIconButton()
			: base()
		{
			this.Click += OnClick;
		}

		private void OnClick(object sender, EventArgs e)
		{
			if (this.DataContext != null)
			{
				var clickMethod = this.DataContext.GetType().GetMethod(this.ClickMethodName);

				var parms = clickMethod.GetParameters();
				clickMethod.Invoke(this.DataContext, null);
			}
		}

		public string ClickMethodName { get; set; }
		public object DataContext { get; set; }
	}
}
