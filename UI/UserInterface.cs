using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;

namespace Terraria.UI;

public class UserInterface
{
	private delegate void MouseElementEvent(UIElement element, UIMouseEvent evt);

	private class InputPointerCache
	{
		public double LastTimeDown;
		public bool WasDown;
		public UIElement LastDown;
		public UIElement LastClicked;
		public MouseElementEvent MouseDownEvent;
		public MouseElementEvent MouseUpEvent;
		public MouseElementEvent ClickEvent;
		public MouseElementEvent DoubleClickEvent;

		public void Clear()
		{
			LastClicked = null;
			LastDown = null;
			LastTimeDown = 0.0;
		}
	}

	private const double DOUBLE_CLICK_TIME = 500.0;
	private const double STATE_CHANGE_CLICK_DISABLE_TIME = 200.0;
	private const int MAX_HISTORY_SIZE = 32;
	private const int HISTORY_PRUNE_SIZE = 4;
	public static UserInterface ActiveInstance = new UserInterface();
	private List<UIState> _history = new List<UIState>();
	private InputPointerCache LeftMouse = new InputPointerCache {
		MouseDownEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.LeftMouseDown(evt);
		},
		MouseUpEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.LeftMouseUp(evt);
		},
		ClickEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.LeftClick(evt);
		},
		DoubleClickEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.LeftDoubleClick(evt);
		}
	};
	private InputPointerCache RightMouse = new InputPointerCache {
		MouseDownEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.RightMouseDown(evt);
		},
		MouseUpEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.RightMouseUp(evt);
		},
		ClickEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.RightClick(evt);
		},
		DoubleClickEvent = delegate (UIElement element, UIMouseEvent evt) {
			element.RightDoubleClick(evt);
		}
	};
	public Vector2 MousePosition;
	private UIElement _lastElementHover;
	private double _clickDisabledTimeRemaining;
	private bool _isStateDirty;
	public bool IsVisible;
	private UIState _currentState;

	public UIState CurrentState => _currentState;

	public void ClearPointers()
	{
		LeftMouse.Clear();
		RightMouse.Clear();
	}

	public void ResetLasts()
	{
		if (_lastElementHover != null)
			_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));

		ClearPointers();
		_lastElementHover = null;
	}

	public void EscapeElements()
	{
		ResetLasts();
	}

	public UserInterface()
	{
		ActiveInstance = this;
	}

	public void Use()
	{
		if (ActiveInstance != this) {
			ActiveInstance = this;
			Recalculate();
		}
		else {
			ActiveInstance = this;
		}
	}

	private void ImmediatelyUpdateInputPointers()
	{
		LeftMouse.WasDown = Main.mouseLeft;
		RightMouse.WasDown = Main.mouseRight;
	}

	private void ResetState()
	{
		if (!Main.dedServ) {
			GetMousePosition();
			ImmediatelyUpdateInputPointers();
			if (_lastElementHover != null)
				_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));
		}

		ClearPointers();
		_lastElementHover = null;
		_clickDisabledTimeRemaining = Math.Max(_clickDisabledTimeRemaining, 200.0);
	}

	private void GetMousePosition()
	{
		MousePosition = new Vector2(Main.mouseX, Main.mouseY);
	}

	public void Update(GameTime time)
	{
		if (_currentState == null)
			return;

		GetMousePosition();
		UIElement uIElement = (Main.hasFocus ? _currentState.GetElementAt(MousePosition) : null);
		_clickDisabledTimeRemaining = Math.Max(0.0, _clickDisabledTimeRemaining - time.ElapsedGameTime.TotalMilliseconds);
		bool num = _clickDisabledTimeRemaining > 0.0;
		if (uIElement != _lastElementHover) {
			if (_lastElementHover != null)
				_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));

			uIElement?.MouseOver(new UIMouseEvent(uIElement, MousePosition));
			_lastElementHover = uIElement;
		}

		if (!num) {
			HandleClick(LeftMouse, time, Main.mouseLeft && Main.hasFocus, uIElement);
			HandleClick(RightMouse, time, Main.mouseRight && Main.hasFocus, uIElement);
		}

		if (PlayerInput.ScrollWheelDeltaForUI != 0) {
			uIElement?.ScrollWheel(new UIScrollWheelEvent(uIElement, MousePosition, PlayerInput.ScrollWheelDeltaForUI));
			PlayerInput.ScrollWheelDeltaForUI = 0;
		}

		if (_currentState != null)
			_currentState.Update(time);
	}

	private void HandleClick(InputPointerCache cache, GameTime time, bool isDown, UIElement mouseElement)
	{
		if (isDown && !cache.WasDown && mouseElement != null) {
			cache.LastDown = mouseElement;
			cache.MouseDownEvent(mouseElement, new UIMouseEvent(mouseElement, MousePosition));
			if (cache.LastClicked == mouseElement && time.TotalGameTime.TotalMilliseconds - cache.LastTimeDown < 500.0) {
				cache.DoubleClickEvent(mouseElement, new UIMouseEvent(mouseElement, MousePosition));
				cache.LastClicked = null;
			}

			cache.LastTimeDown = time.TotalGameTime.TotalMilliseconds;
		}
		else if (!isDown && cache.WasDown && cache.LastDown != null) {
			UIElement lastDown = cache.LastDown;
			if (lastDown.ContainsPoint(MousePosition)) {
				cache.ClickEvent(lastDown, new UIMouseEvent(lastDown, MousePosition));
				cache.LastClicked = cache.LastDown;
			}

			cache.MouseUpEvent(lastDown, new UIMouseEvent(lastDown, MousePosition));
			cache.LastDown = null;
		}

		cache.WasDown = isDown;
	}

	public void Draw(SpriteBatch spriteBatch, GameTime time)
	{
		Use();
		if (_currentState != null) {
			if (_isStateDirty) {
				_currentState.Recalculate();
				_isStateDirty = false;
			}

			_currentState.Draw(spriteBatch);
		}
	}

	public void DrawDebugHitbox(BasicDebugDrawer drawer)
	{
		_ = _currentState;
	}

	public void SetState(UIState state)
	{
		if (state == _currentState)
			return;

		if (state != null)
			AddToHistory(state);

		if (_currentState != null) {
			if (_lastElementHover != null)
				_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));

			_currentState.Deactivate();
		}

		_currentState = state;
		ResetState();
		if (state != null) {
			_isStateDirty = true;
			state.Activate();
			state.Recalculate();
		}
	}

	public void GoBack()
	{
		if (_history.Count >= 2) {
			UIState state = _history[_history.Count - 2];
			_history.RemoveRange(_history.Count - 2, 2);
			SetState(state);
		}
	}

	private void AddToHistory(UIState state)
	{
		_history.Add(state);
		if (_history.Count > 32)
			_history.RemoveRange(0, 4);
	}

	public void Recalculate()
	{
		if (_currentState != null)
			_currentState.Recalculate();
	}

	public CalculatedStyle GetDimensions()
	{
		Vector2 originalScreenSize = PlayerInput.OriginalScreenSize;
		return new CalculatedStyle(0f, 0f, originalScreenSize.X / Main.UIScale, originalScreenSize.Y / Main.UIScale);
	}

	internal void RefreshState()
	{
		if (_currentState != null)
			_currentState.Deactivate();

		ResetState();
		_currentState.Activate();
		_currentState.Recalculate();
	}

	public bool IsElementUnderMouse()
	{
		if (IsVisible && _lastElementHover != null)
			return !(_lastElementHover is UIState);

		return false;
	}
}
