using System;
using Windows.UI.Popups;
using Windows.UI.StartScreen;

namespace Charmed
{
	public sealed class TileInfo
	{
		/// <summary>
		/// Constructor containing info for pinning a tile.
		/// </summary>
		/// <param name="tileId">The Id of the tile to pin</param>
		/// <param name="shortName">The short name for the tile</param>
		/// <param name="displayName">The display name for the tile</param>
		/// <param name="tileOptions">The TileOptions for the tile</param>
		/// <param name="logoUri">The Uri to the tile logo.</param>
		/// <param name="arguments">Optional arguments to provide for when the tile is activated.</param>
		public TileInfo(
			string tileId,
			string shortName,
			string displayName,
			TileOptions tileOptions,
			Uri logoUri,
			string arguments = null)
		{
			this.TileId = tileId;
			this.ShortName = shortName;
			this.DisplayName = displayName;
			this.Arguments = arguments;
			this.TileOptions = tileOptions;
			this.LogoUri = logoUri;
			this.Arguments = arguments;
		}

		/// <summary>
		/// Constructor containing info for pinning a tile.
		/// </summary>
		/// <param name="tileId">The Id of the tile to pin</param>
		/// <param name="shortName">The short name for the tile</param>
		/// <param name="displayName">The display name for the tile</param>
		/// <param name="tileOptions">The TileOptions for the tile</param>
		/// <param name="logoUri">The Uri to the tile logo.</param>
		/// <param name="wideLogoUri">The Uri to the wide tile logo.</param>
		/// <param name="arguments">Optional arguments to provide for when the tile is activated.</param>
		public TileInfo(
			string tileId,
			string shortName,
			string displayName,
			TileOptions tileOptions,
			Uri logoUri,
			Uri wideLogoUri,
			string arguments = null)
		{
			this.TileId = tileId;
			this.ShortName = shortName;
			this.DisplayName = displayName;
			this.Arguments = arguments;
			this.TileOptions = tileOptions;
			this.LogoUri = logoUri;
			this.WideLogoUri = wideLogoUri;
			this.Arguments = arguments;
		}

		public string TileId { get; set; }
		public string ShortName { get; set; }
		public string DisplayName { get; set; }
		public string Arguments { get; set; }
		public TileOptions TileOptions { get; set; }
		public Uri LogoUri { get; set; }
		public Uri WideLogoUri { get; set; }
	}
}
