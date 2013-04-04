using System;
using Windows.ApplicationModel.DataTransfer;

namespace Charmed
{
	public interface IShareManager
	{
		void Initialize();
		void Cleanup();
		Action<DataPackage> OnShareRequested { get; set; }
	}
}
