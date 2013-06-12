using System.Threading.Tasks;

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
		/// <param name="tileInfo">The TileInfo object containing all the information about the tile to pin.</param>
		/// <returns>Returns true if the tile was pinned and false if it was not pinned.</returns>
		Task<bool> Pin(TileInfo tileInfo);

		/// <summary>
		/// Unpins a secondary tile.
		/// </summary>
		/// <param name="tileInfo">The TileInfo object containing all the information about the tile to unpin.</param>
		/// <returns>Returns true if the tile was unpinned and false if it was not unpinned.</returns>
		Task<bool> Unpin(TileInfo tileInfo);

		/// <summary>
		/// Checks if a tile is already pinned.
		/// </summary>
		/// <param name="tileId">The Id of the tile to check for.</param>
		/// <returns>Returns true if the tile is already pinned and false if not.</returns>
		bool IsPinned(string tileId);
	}
}
