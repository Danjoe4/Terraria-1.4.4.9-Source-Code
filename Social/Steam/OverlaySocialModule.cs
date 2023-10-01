using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam;

public class OverlaySocialModule : Terraria.Social.Base.OverlaySocialModule
{
	private Callback<GamepadTextInputDismissed_t> _gamepadTextInputDismissed;
	private bool _gamepadTextInputActive;

	public override void Initialize()
	{
		_gamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed);
	}

	public override void Shutdown()
	{
	}

	public override bool IsGamepadTextInputActive() => _gamepadTextInputActive;

	public override bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false, string existingText = "", bool password = false)
	{
		if (_gamepadTextInputActive)
			return false;

		bool num = SteamUtils.ShowGamepadTextInput(password ? EGamepadTextInputMode.k_EGamepadTextInputModePassword : EGamepadTextInputMode.k_EGamepadTextInputModeNormal, multiLine ? EGamepadTextInputLineMode.k_EGamepadTextInputLineModeMultipleLines : EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, description, maxLength, existingText);
		if (num)
			_gamepadTextInputActive = true;

		return num;
	}

	public override string GetGamepadText()
	{
		uint enteredGamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
		SteamUtils.GetEnteredGamepadTextInput(out var pchText, enteredGamepadTextLength);
		return pchText;
	}

	private void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t result)
	{
		_gamepadTextInputActive = false;
	}
}
