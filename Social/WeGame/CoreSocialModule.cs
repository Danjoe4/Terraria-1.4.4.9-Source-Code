using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using rail;
using ReLogic.OS;

namespace Terraria.Social.WeGame;

public class CoreSocialModule : ISocialModule
{
	private RailCallBackHelper _callbackHelper = new RailCallBackHelper();
	private static object _railTickLock = new object();
	private bool isRailValid;

	public static event Action OnTick;

	[DllImport("kernel32.dll", ExactSpelling = true)]
	private static extern uint GetCurrentThreadId();

	public void Initialize()
	{
		RailGameID railGameID = new RailGameID();
		railGameID.id_ = 2000328uL;
		string[] array = ((!Main.dedServ) ? new string[1] {
			" "
		} : Environment.GetCommandLineArgs());

		if (rail_api.RailNeedRestartAppForCheckingEnvironment(railGameID, array.Length, array))
			Environment.Exit(1);

		if (!rail_api.RailInitialize())
			Environment.Exit(1);

		_callbackHelper.RegisterCallback(RAILEventID.kRailEventSystemStateChanged, RailEventCallBack);
		isRailValid = true;
		ThreadPool.QueueUserWorkItem(TickThread, null);
		Main.OnTickForThirdPartySoftwareOnly += RailEventTick;
	}

	public static void RailEventTick()
	{
		rail_api.RailFireEvents();
		if (Monitor.TryEnter(_railTickLock)) {
			Monitor.Pulse(_railTickLock);
			Monitor.Exit(_railTickLock);
		}
	}

	private void TickThread(object context)
	{
		Monitor.Enter(_railTickLock);
		while (isRailValid) {
			if (CoreSocialModule.OnTick != null)
				CoreSocialModule.OnTick();

			Monitor.Wait(_railTickLock);
		}

		Monitor.Exit(_railTickLock);
	}

	public void Shutdown()
	{
		if (Platform.IsWindows) {
			Application.ApplicationExit += delegate {
				isRailValid = false;
			};
		}
		else {
			isRailValid = false;
			AppDomain.CurrentDomain.ProcessExit += delegate {
				isRailValid = false;
			};
		}

		_callbackHelper.UnregisterAllCallback();
		rail_api.RailFinalize();
	}

	public static void RailEventCallBack(RAILEventID eventId, EventBase data)
	{
		if (eventId == RAILEventID.kRailEventSystemStateChanged)
			ProcessRailSystemStateChange(((RailSystemStateChanged)data).state);
	}

	public static void SaveAndQuitCallBack()
	{
		Main.WeGameRequireExitGame();
	}

	private static void ProcessRailSystemStateChange(RailSystemState state)
	{
		if (state == RailSystemState.kSystemStatePlatformOffline || state == RailSystemState.kSystemStatePlatformExit) {
			MessageBox.Show("检测到WeGame异常，游戏将自动保存进度并退出游戏", "Terraria--WeGame Error");
			WorldGen.SaveAndQuit(SaveAndQuitCallBack);
		}
	}
}
