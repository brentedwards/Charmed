using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Charmed.Sample.Tests.Mocks
{
	public class SecondaryPinnerMock : ISecondaryPinner
	{
		public Func<FrameworkElement, Placement, TileInfo, bool> PinDelegate { get; set; }
		public Task<bool> Pin(FrameworkElement anchorElement, Placement requestPlacement, TileInfo tileInfo)
		{
			return MockHelper.ExecuteDelegateAsync<FrameworkElement, Placement, TileInfo, bool>(
				this.PinDelegate,
				anchorElement,
				requestPlacement,
				tileInfo);
		}

		public Func<FrameworkElement, Placement, string, bool> UnpinDelegate { get; set; }
		public Task<bool> Unpin(FrameworkElement anchorElement, Placement requestPlacement, string tileId)
		{
			return MockHelper.ExecuteDelegateAsync<FrameworkElement, Placement, string, bool>(
				this.UnpinDelegate,
				anchorElement,
				requestPlacement,
				tileId);
		}

		public Func<string, bool> IsPinnedDelegate { get; set; }
		public bool IsPinned(string tileId)
		{
			return MockHelper.ExecuteDelegate<string, bool>(this.IsPinnedDelegate, tileId);
		}
	}
}
