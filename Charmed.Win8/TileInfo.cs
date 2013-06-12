using System;
using Windows.UI.Popups;

namespace Charmed
{
	public sealed class TileInfo
	{
#if NETFX_CORE
		/// <summary>
		/// Constructor containing info for pinning a tile.
		/// </summary>
		/// <param name="tileId">The Id of the tile to pin</param>
		/// <param name="shortName">The short name for the tile</param>
		/// <param name="displayName">The display name for the tile</param>
		/// <param name="tileOptions">The TileOptions for the tile</param>
		/// <param name="logoUri">The Uri to the tile logo.</param>
		/// <param name="anchorElement">The anchor element that the pin request dialog will display next to.</param>
		/// <param name="requestPlacement">The Placement value that tells where the pin request dialog displays in relation to anchorElement.</param>
		/// <param name="arguments">Optional arguments to provide for when the tile is activated.</param>
		public TileInfo(
			string tileId,
			string shortName,
			string displayName,
			Windows.UI.StartScreen.TileOptions tileOptions,
			Uri logoUri,
			Windows.UI.Xaml.FrameworkElement anchorElement,
			Placement requestPlacement,
			string arguments = null)
		{
			this.TileId = tileId;
			this.ShortName = shortName;
			this.DisplayName = displayName;
			this.Arguments = arguments;
			this.TileOptions = tileOptions;
			this.LogoUri = logoUri;

			this.AnchorElement = anchorElement;
			this.RequestPlacement = requestPlacement;

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
		/// <param name="anchorElement">The anchor element that the pin request dialog will display next to.</param>
		/// <param name="requestPlacement">The Placement value that tells where the pin request dialog displays in relation to anchorElement.</param>
		/// <param name="arguments">Optional arguments to provide for when the tile is activated.</param>
		public TileInfo(
			string tileId,
			string shortName,
			string displayName,
			Windows.UI.StartScreen.TileOptions tileOptions,
			Uri logoUri,
			Uri wideLogoUri,
			Windows.UI.Xaml.FrameworkElement anchorElement,
			Placement requestPlacement,
			string arguments = null)
		{
			this.TileId = tileId;
			this.ShortName = shortName;
			this.DisplayName = displayName;
			this.Arguments = arguments;
			this.TileOptions = tileOptions;
			this.LogoUri = logoUri;
			this.WideLogoUri = wideLogoUri;

			this.AnchorElement = anchorElement;
			this.RequestPlacement = requestPlacement;

			this.Arguments = arguments;
		}

		/// <summary>
		/// Constructor containing info for unpinning a tile.
		/// </summary>
		/// <param name="tileId">The Id of the tile to pin</param>
		/// <param name="anchorElement">The anchor element that the pin request dialog will display next to.</param>
		/// <param name="requestPlacement">The Placement value that tells where the pin request dialog displays in relation to anchorElement.</param>
		public TileInfo(
			string tileId,
			Windows.UI.Xaml.FrameworkElement anchorElement,
			Placement requestPlacement)
		{
			this.TileId = tileId;

			this.AnchorElement = anchorElement;
			this.RequestPlacement = requestPlacement;
		}
#endif // NETFX_CORE

		public string TileId { get; set; }
		public string ShortName { get; set; }
		public string DisplayName { get; set; }
		public string Arguments { get; set; }
		public Uri LogoUri { get; set; }
		public Uri WideLogoUri { get; set; }

		public int? Count { get; set; }

#if NETFX_CORE
		public Windows.UI.StartScreen.TileOptions TileOptions { get; set; }
		public Windows.UI.Xaml.FrameworkElement AnchorElement { get; set; }
		public Placement RequestPlacement { get; set; }
#endif // NETFX_CORE
	}
}
