using Terraria.UI;

namespace Terraria.GameContent.Bestiary;

public class BestiaryPortraitBackgroundProviderPreferenceInfoElement : IPreferenceProviderElement, IBestiaryInfoElement
{
	private IBestiaryBackgroundImagePathAndColorProvider _preferredProvider;

	public BestiaryPortraitBackgroundProviderPreferenceInfoElement(IBestiaryBackgroundImagePathAndColorProvider preferredProvider)
	{
		_preferredProvider = preferredProvider;
	}

	public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => null;
	public bool Matches(IBestiaryBackgroundImagePathAndColorProvider provider) => provider == _preferredProvider;
	public IBestiaryBackgroundImagePathAndColorProvider GetPreferredProvider() => _preferredProvider;
}
