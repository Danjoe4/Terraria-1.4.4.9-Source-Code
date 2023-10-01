using Terraria.IO;

namespace Terraria.Social.Base;

public class TexturePackWorkshopEntry : AWorkshopEntry
{
	public static string GetHeaderTextFor(ResourcePack resourcePack, ulong workshopEntryId, string[] tags, WorkshopItemPublicSettingId publicity, string previewImagePath) => AWorkshopEntry.CreateHeaderJson("ResourcePack", workshopEntryId, tags, publicity, previewImagePath);
}
