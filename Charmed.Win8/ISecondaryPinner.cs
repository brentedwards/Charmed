using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Charmed
{
	/// <summary>
	/// Interface for pinning a secondary tile.
	/// </summary>
	public interface ISecondaryPinner
	{
		/// <summary>
		/// Pins a secondary tile.
		/// </summary>
		/// <param name="anchorElement">The anchor element that the pin request dialog will display next to.</param>
		/// <param name="requestPlacement">The Placement value that tells where the pin request dialog displays in relation to anchorElement.</param>
		/// <param name="tileInfo">The TileInfo object containing all the information about the tile to pin.</param>
		/// <returns>Returns true if the tile was pinned and false if it was not pinned.</returns>
		Task<bool> Pin(FrameworkElement anchorElement, Placement requestPlacement, TileInfo tileInfo);

		/// <summary>
		/// Unpins a secondary tile.
		/// </summary>
		/// <param name="anchorElement">The anchor element that the unpin request dialog will display next to.</param>
		/// <param name="requestPlacement">The Placement value that tells where the unpin request dialog displays in relation to anchorElement.</param>
		/// <param name="tileId">The Id of the tile to unpin.</param>
		/// <returns>Returns true if the tile was unpinned and false if it was not unpinned.</returns>
		Task<bool> Unpin(FrameworkElement anchorElement, Placement requestPlacement, string tileId);

		/// <summary>
		/// Checks if a tile is already pinned.
		/// </summary>
		/// <param name="tileId">The Id of the tile to check for.</param>
		/// <returns>Returns true if the tile is already pinned and false if not.</returns>
		bool IsPinned(string tileId);
	}
}
