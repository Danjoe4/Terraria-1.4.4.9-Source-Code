using Terraria.IO;

namespace Terraria.Social.Base;

public class WorldWorkshopEntry : AWorkshopEntry
{
	public static string GetHeaderTextFor(WorldFileData world, ulong workshopEntryId, string[] tags, WorkshopItemPublicSettingId publicity, string previewImagePath) => AWorkshopEntry.CreateHeaderJson("World", workshopEntryId, tags, publicity, previewImagePath);
}
