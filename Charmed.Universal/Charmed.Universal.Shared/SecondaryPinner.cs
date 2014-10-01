using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Charmed
{
	public sealed class SecondaryPinner : ISecondaryPinner
	{
		public async Task<bool> Pin(TileInfo tileInfo)
		{
			if (tileInfo == null)
			{
				throw new ArgumentNullException("tileInfo");
			}

			var isPinned = false;

			if (!SecondaryTile.Exists(tileInfo.TileId))
			{
				var secondaryTile = new SecondaryTile(
					tileInfo.TileId,
					tileInfo.DisplayName,
					tileInfo.Arguments,
					tileInfo.LogoUri,
					tileInfo.TileSize);

				if (tileInfo.WideLogoUri != null)
				{
					secondaryTile.VisualElements.Wide310x150Logo = tileInfo.WideLogoUri;
				}

				secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
				secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
				secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;

				isPinned = await secondaryTile.RequestCreateForSelectionAsync(
						GetElementRect(tileInfo.AnchorElement), tileInfo.RequestPlacement);
			}

			return isPinned;
		}

		public async Task<bool> Unpin(TileInfo tileInfo)
		{
			var wasUnpinned = false;

			if (SecondaryTile.Exists(tileInfo.TileId))
			{
				var secondaryTile = new SecondaryTile(tileInfo.TileId);
				wasUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(
					GetElementRect(tileInfo.AnchorElement), tileInfo.RequestPlacement);
			}

			return wasUnpinned;
		}

		public bool IsPinned(string tileId)
		{
			return SecondaryTile.Exists(tileId);
		}

		private static Rect GetElementRect(FrameworkElement element)
		{
			GeneralTransform buttonTransform = element.TransformToVisual(null);
			Point point = buttonTransform.TransformPoint(new Point());
			return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
		}
	}
}
