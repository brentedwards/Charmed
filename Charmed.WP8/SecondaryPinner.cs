using Microsoft.Phone.Shell;
using System;
using System.Linq;
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
					BackTitle = tileInfo.AppName,
					BackBackgroundImage = new Uri("", UriKind.Relative),
					BackContent = tileInfo.DisplayName
				};

				ShellTile.Create(new Uri(tileInfo.TileId, UriKind.Relative), tileData);
				result = true;
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

			return Task.FromResult<bool>(true);
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
