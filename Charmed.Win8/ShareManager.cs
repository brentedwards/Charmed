using System;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed
{
	public sealed class ShareManager : IShareManager
	{
		private DataTransferManager DataTransferManager { get; set; }

		public void Initialize()
		{
			this.DataTransferManager = DataTransferManager.GetForCurrentView();
			this.DataTransferManager.DataRequested += this.DataTransferManager_DataRequested;
		}

		public void Cleanup()
		{
			this.DataTransferManager.DataRequested -= this.DataTransferManager_DataRequested;
		}

		private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
		{
			if (this.OnShareRequested != null)
			{
				this.OnShareRequested(args.Request.Data);
			}
		}

		public Action<DataPackage> OnShareRequested { get; set; }
	}
}
