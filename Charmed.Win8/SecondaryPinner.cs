using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Charmed
{
	public sealed class SecondaryPinner : ISecondaryPinner
	{
		public async Task<bool> Pin(
			FrameworkElement anchorElement,
			Placement requestPlacement,
			TileInfo tileInfo)
		{
			if (anchorElement == null)
			{
				throw new ArgumentNullException("anchorElement");
			}
			if (tileInfo == null)
			{
				throw new ArgumentNullException("tileInfo");
			}

			var isPinned = false;

			if (!SecondaryTile.Exists(tileInfo.TileId))
			{
				var secondaryTile = new SecondaryTile(
					tileInfo.TileId,
					tileInfo.ShortName,
					tileInfo.DisplayName,
					tileInfo.Arguments,
					tileInfo.TileOptions,
					tileInfo.LogoUri,
					tileInfo.WideLogoUri);

				isPinned = await secondaryTile.RequestCreateForSelectionAsync(
						GetElementRect(anchorElement), requestPlacement);
			}

			return isPinned;
		}

		public async Task<bool> Unpin(
			FrameworkElement anchorElement,
			Placement requestPlacement,
			string tileId)
		{
			var wasUnpinned = false;

			if (SecondaryTile.Exists(tileId))
			{
				var secondaryTile = new SecondaryTile(tileId);
				wasUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(
					GetElementRect(anchorElement), requestPlacement);
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
