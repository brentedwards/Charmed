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
				if (this.DataItem != null && parms != null && parms.Length == 1)
				{
					var param = parms[0];
					var paramType = param.ParameterType;

					var dataItemType = this.DataItem.GetType();
					if (dataItemType != paramType)
					{
						throw new ArgumentException("DataItem is the wrong type.");
					}

					clickMethod.Invoke(this.DataContext, new[] { this.DataItem });
				}
				else
				{
					clickMethod.Invoke(this.DataContext, null);
				}
			}
		}

		public string ClickMethodName { get; set; }
		public object DataContext { get; set; }
		public object DataItem { get; set; }
	}
}
