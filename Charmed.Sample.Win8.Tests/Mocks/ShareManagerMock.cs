using System;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed.Sample.Tests.Mocks
{
	public class ShareManagerMock : IShareManager
	{
		public Action InitializeDelegate { get; set; }
		public void Initialize()
		{
			if (this.InitializeDelegate != null)
			{
				this.InitializeDelegate();
			}
		}

		public Action CleanupDelegate { get; set; }
		public void Cleanup()
		{
			if (this.CleanupDelegate != null)
			{
				this.CleanupDelegate();
			}
		}

		public Action<DataPackage> OnShareRequested { get; set; }
	}
}
