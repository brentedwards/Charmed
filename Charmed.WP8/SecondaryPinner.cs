using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed
{
	public sealed class SecondaryPinner : ISecondaryPinner
	{
		public Task<bool> Pin(TileInfo tileInfo)
		{
			var result = false;
			if (!this.IsPinned(tileInfo.TileId))
			{
				var tileData = new StandardTileData
				{
					Title = tileInfo.DisplayName,
					BackgroundImage = tileInfo.LogoUri,
					Count = tileInfo.Count,
					BackTitle = tileInfo.DisplayName,
					BackBackgroundImage = new Uri("", UriKind.Relative),
					BackContent = tileInfo.DisplayName // TODO: Should this be the app name?
				};

				ShellTile.Create(new Uri(tileInfo.TileId, UriKind.Relative), tileData);
			}

			return Task.FromResult<bool>(result);
		}

		public Task<bool> Unpin(TileInfo tileInfo)
		{
			ShellTile tile = this.FindTile(tileInfo.TileId);
			if (tile != null)
			{
				tile.Delete();
			}

			return Task.FromResult<bool>(false);
		}

		public bool IsPinned(string tileId)
		{
			return FindTile(tileId) != null;
		}

		private ShellTile FindTile(string uri)
		{
			return ShellTile.ActiveTiles.FirstOrDefault(tile => tile.NavigationUri.ToString() == uri);
		}
	}
}
