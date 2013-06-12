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
		public Func<TileInfo, bool> PinDelegate { get; set; }
		public Task<bool> Pin(TileInfo tileInfo)
		{
			return MockHelper.ExecuteDelegateAsync<TileInfo, bool>(
				this.PinDelegate,
				tileInfo);
		}

		public Func<TileInfo, bool> UnpinDelegate { get; set; }
		public Task<bool> Unpin(TileInfo tileInfo)
		{
			return MockHelper.ExecuteDelegateAsync<TileInfo, bool>(
				this.UnpinDelegate,
				tileInfo);
		}

		public Func<string, bool> IsPinnedDelegate { get; set; }
		public bool IsPinned(string tileId)
		{
			return MockHelper.ExecuteDelegate<string, bool>(this.IsPinnedDelegate, tileId);
		}
	}
}
