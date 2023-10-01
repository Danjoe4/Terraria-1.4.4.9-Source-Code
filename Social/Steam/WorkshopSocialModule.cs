using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Social.Base;

namespace Terraria.Social.Steam;

public class WorkshopSocialModule : Terraria.Social.Base.WorkshopSocialModule
{
	private WorkshopHelper.UGCBased.Downloader _downloader;
	private WorkshopHelper.UGCBased.PublishedItemsFinder _publishedItems;
	private List<WorkshopHelper.UGCBased.APublisherInstance> _publisherInstances;
	private string _contentBaseFolder;

	public override void Initialize()
	{
		base.Branding = new WorkshopBranding {
			ResourcePackBrand = ResourcePack.BrandingType.SteamWorkshop
		};

		_publisherInstances = new List<WorkshopHelper.UGCBased.APublisherInstance>();
		base.ProgressReporter = new WorkshopProgressReporter(_publisherInstances);
		base.SupportedTags = new SupportedWorkshopTags();
		_contentBaseFolder = Main.SavePath + Path.DirectorySeparatorChar + "Workshop";
		_downloader = WorkshopHelper.UGCBased.Downloader.Create();
		_publishedItems = WorkshopHelper.UGCBased.PublishedItemsFinder.Create();
		WorkshopIssueReporter workshopIssueReporter = new WorkshopIssueReporter();
		workshopIssueReporter.OnNeedToOpenUI += _issueReporter_OnNeedToOpenUI;
		workshopIssueReporter.OnNeedToNotifyUI += _issueReporter_OnNeedToNotifyUI;
		base.IssueReporter = workshopIssueReporter;
		UIWorkshopHub.OnWorkshopHubMenuOpened += RefreshSubscriptionsAndPublishings;
	}

	private void _issueReporter_OnNeedToNotifyUI()
	{
		Main.IssueReporterIndicator.AttemptLettingPlayerKnow();
		Main.WorkshopPublishingIndicator.Hide();
	}

	private void _issueReporter_OnNeedToOpenUI()
	{
		Main.OpenReportsMenu();
	}

	public override void Shutdown()
	{
	}

	public override void LoadEarlyContent()
	{
		RefreshSubscriptionsAndPublishings();
	}

	private void RefreshSubscriptionsAndPublishings()
	{
		_downloader.Refresh(base.IssueReporter);
		_publishedItems.Refresh();
	}

	public override List<string> GetListOfSubscribedWorldPaths() => _downloader.WorldPaths.Select((string folderPath) => folderPath + Path.DirectorySeparatorChar + "world.wld").ToList();
	public override List<string> GetListOfSubscribedResourcePackPaths() => _downloader.ResourcePackPaths;

	public override bool TryGetPath(string pathEnd, out string fullPathFound)
	{
		fullPathFound = null;
		string text = _downloader.ResourcePackPaths.FirstOrDefault((string x) => x.EndsWith(pathEnd));
		if (text == null)
			return false;

		fullPathFound = text;
		return true;
	}

	private void Forget(WorkshopHelper.UGCBased.APublisherInstance instance)
	{
		_publisherInstances.Remove(instance);
		RefreshSubscriptionsAndPublishings();
	}

	public override void PublishWorld(WorldFileData world, WorkshopItemPublishSettings settings)
	{
		string name = world.Name;
		string textForWorld = GetTextForWorld(world);
		string[] usedTagsInternalNames = settings.GetUsedTagsInternalNames();
		string text = GetTemporaryFolderPath() + world.GetFileName(includeExtension: false);
		if (MakeTemporaryFolder(text)) {
			WorkshopHelper.UGCBased.WorldPublisherInstance worldPublisherInstance = new WorkshopHelper.UGCBased.WorldPublisherInstance(world);
			_publisherInstances.Add(worldPublisherInstance);
			worldPublisherInstance.PublishContent(_publishedItems, base.IssueReporter, Forget, name, textForWorld, text, settings.PreviewImagePath, settings.Publicity, usedTagsInternalNames);
		}
	}

	private string GetTextForWorld(WorldFileData world)
	{
		string text = "This is \"";
		text += world.Name;
		string text2 = "";
		switch (world.WorldSizeX) {
			case 4200:
				text2 = "small";
				break;
			case 6400:
				text2 = "medium";
				break;
			case 8400:
				text2 = "large";
				break;
			default:
				text2 = "custom";
				break;
		}

		string text3 = "";
		switch (world.GameMode) {
			case 3:
				text3 = "journey";
				break;
			case 0:
				text3 = "classic";
				break;
			case 1:
				text3 = "expert";
				break;
			case 2:
				text3 = "master";
				break;
			default:
				text3 = "custom";
				break;
		}

		text = text + "\", a " + text2.ToLower() + " " + text3.ToLower() + " world";
		text = text + " infected by the " + (world.HasCorruption ? "corruption" : "crimson");
		if (world.IsHardMode)
			text += ", in hardmode";

		return text + ".";
	}

	public override void PublishResourcePack(ResourcePack resourcePack, WorkshopItemPublishSettings settings)
	{
		if (resourcePack.IsCompressed) {
			base.IssueReporter.ReportInstantUploadProblem("Workshop.ReportIssue_CannotPublishZips");
			return;
		}

		string name = resourcePack.Name;
		string text = resourcePack.Description;
		if (string.IsNullOrWhiteSpace(text))
			text = "";

		string[] usedTagsInternalNames = settings.GetUsedTagsInternalNames();
		string fullPath = resourcePack.FullPath;
		WorkshopHelper.UGCBased.ResourcePackPublisherInstance resourcePackPublisherInstance = new WorkshopHelper.UGCBased.ResourcePackPublisherInstance(resourcePack);
		_publisherInstances.Add(resourcePackPublisherInstance);
		resourcePackPublisherInstance.PublishContent(_publishedItems, base.IssueReporter, Forget, name, text, fullPath, settings.PreviewImagePath, settings.Publicity, usedTagsInternalNames);
	}

	private string GetTemporaryFolderPath() => string.Concat(str2: SteamUser.GetSteamID().m_SteamID.ToString(), str0: _contentBaseFolder, str1: Path.DirectorySeparatorChar.ToString(), str3: Path.DirectorySeparatorChar.ToString());

	private bool MakeTemporaryFolder(string temporaryFolderPath)
	{
		bool result = true;
		if (!Utils.TryCreatingDirectory(temporaryFolderPath)) {
			base.IssueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_CouldNotCreateTemporaryFolder!");
			result = false;
		}

		return result;
	}

	public override void ImportDownloadedWorldToLocalSaves(WorldFileData world, string newFileName = null, string newDisplayName = null)
	{
		Main.menuMode = 10;
		world.CopyToLocal(newFileName, newDisplayName);
	}

	public List<IssueReport> GetReports()
	{
		List<IssueReport> list = new List<IssueReport>();
		if (base.IssueReporter != null)
			list.AddRange(base.IssueReporter.GetReports());

		return list;
	}

	public override bool TryGetInfoForWorld(WorldFileData world, out FoundWorkshopEntryInfo info)
	{
		info = null;
		string text = GetTemporaryFolderPath() + world.GetFileName(includeExtension: false);
		if (!Directory.Exists(text))
			return false;

		if (AWorkshopEntry.TryReadingManifest(text + Path.DirectorySeparatorChar + "workshop.json", out info))
			return true;

		return false;
	}

	public override bool TryGetInfoForResourcePack(ResourcePack resourcePack, out FoundWorkshopEntryInfo info)
	{
		info = null;
		string fullPath = resourcePack.FullPath;
		if (!Directory.Exists(fullPath))
			return false;

		if (AWorkshopEntry.TryReadingManifest(fullPath + Path.DirectorySeparatorChar + "workshop.json", out info))
			return true;

		return false;
	}
}
