using Microsoft.Phone.Shell;
using System;

namespace Charmed.ApplicationBar
{
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
