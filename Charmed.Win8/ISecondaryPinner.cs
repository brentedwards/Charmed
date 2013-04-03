using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Charmed
{
	public interface ISecondaryPinner
	{
		Task<bool> Pin(FrameworkElement anchorElement, Placement requestPlacement, TileInfo tileInfo);
		Task<bool> Unpin(FrameworkElement anchorElement, Placement requestPlacement, string tileId);
		bool IsPinned(string tileId);
	}
}
