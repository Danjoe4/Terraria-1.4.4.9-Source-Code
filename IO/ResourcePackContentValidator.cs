using System.IO;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.IO;

public class ResourcePackContentValidator
{
	public void ValidateResourePack(ResourcePack pack)
	{
		if ((AssetReaderCollection)Main.instance.Services.GetService(typeof(AssetReaderCollection)) != null) {
			pack.GetContentSource().GetAllAssetsStartingWith("Images" + Path.DirectorySeparatorChar);
			VanillaContentValidator.Instance.GetValidImageFilePaths();
		}
	}
}
