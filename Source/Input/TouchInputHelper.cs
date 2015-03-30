using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MouseBuddy;
using ResolutionBuddy;
using TouchScreenBuddy;

namespace MenuBuddy
{
	/// <summary>
	/// This is an input helper that does mouse/touchscreen input
	/// </summary>
	public class TouchInputHelper : DrawableGameComponent, IInputHelper
	{
		#region Properties

		/// <summary>
		/// Get the mouse position in game coords
		/// </summary>
		public Vector2 MousePos
		{
			get
			{
				Vector2 mouse = Vector2.Zero;
				if (null != MouseManager)
				{
					mouse = Resolution.ScreenToGameCoord(MouseManager.MousePos);
				}
				return mouse;
			}
		}

		public IMouseManager MouseManager { get; private set; }

		/// <summary>
		/// the touch manager service component.
		/// warning: this dude might be null if the compoent isnt in this game
		/// </summary>
		public ITouchManager TouchManager { get; private set; }

		private SpriteBatch SpriteBatch { get; set; }

		private DrawHelper DrawHelper { get; set; }

		#endregion //Properties

		#region Initialization

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="game"></param>
		public TouchInputHelper(Game game)
			: base(game)
		{
			//Find all the components we need
			TouchManager = game.Services.GetService(typeof(ITouchManager)) as ITouchManager;
			MouseManager = game.Services.GetService(typeof(IMouseManager)) as IMouseManager;

			//make sure that stuff was init correctly
			Debug.Assert((null != TouchManager) || (null != MouseManager));

			//Register ourselves to implement the DI container service.
			game.Components.Add(this);
			game.Services.AddService(typeof(IInputHelper), this);
		}

		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			DrawHelper = new DrawHelper(GraphicsDevice, SpriteBatch);
		}

		#endregion //Initialization

		#region Methods

		public void HandleInput(IScreen screen)
		{
			var widgetScreen = screen as WidgetScreen;
			if (null != widgetScreen)
			{
				//Get all the widgets from the screen
				var buttons = widgetScreen.Buttons;

				if (null != MouseManager)
				{
					//check if the player is holding the LMouseButton down in the widget
					if (MouseManager.LMouseDown)
					{
						CheckHighlight(buttons, MouseManager.MousePos);
					}

					//check if the player selected an item
					if (MouseManager.LMouseClick)
					{
						CheckClick(widgetScreen, buttons, MouseManager.MousePos);
					}
				}

				if (null != TouchManager)
				{
					//check if the player is holding down on the screen
					foreach (var touch in TouchManager.Touches)
					{
						CheckHighlight(buttons, touch);
					}

					//check if the player tapped on the screen
					foreach (var tap in TouchManager.Taps)
					{
						CheckClick(widgetScreen, buttons, tap);
					}
				}
			}
		}

		private void CheckHighlight(IEnumerable<IButton> buttons, Vector2 point)
		{
			foreach (var button in buttons)
			{
				if (button.Rect.Contains(point))
				{
					OnButtonHighlighted(button);
				}
				else
				{
					OnButtonNotHighlighted(button);
				}
			}
		}

		private void CheckClick(WidgetScreen screen, IEnumerable<IButton> buttons, Vector2 point)
		{
			foreach (var button in buttons)
			{
				if (button.Rect.Contains(point))
				{
					OnButtonClick(screen, button);
					break;
				}
			}
		}

		/// <summary>
		/// Called every time update while a widget is highlighted
		/// </summary>
		/// <param name="button"></param>
		private void OnButtonHighlighted(IButton button)
		{
			button.IsHighlighted = true;
		}

		private void OnButtonNotHighlighted(IButton button)
		{
			button.IsHighlighted = false;
		}

		/// <summary>
		/// Called when a widget is selected
		/// </summary>
		/// <param name="screen"></param>
		/// <param name="button"></param>
		private void OnButtonClick(WidgetScreen screen, IButton button)
		{
			//run the selected event
			button.OnSelect(null);

			//tell the menu too
			screen.OnSelect(null);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

#if DEBUG
			//draw a circle around the mouse cursor
			if (null != MouseManager)
			{
				var mouse = Mouse.GetState();
				var mousePos = new Vector2(mouse.X, mouse.Y);

				SpriteBatch.Begin();

				DrawHelper.Prim.NumCircleSegments = 4;
				DrawHelper.Prim.Circle(mousePos, 6.0f, Color.Red);

				SpriteBatch.End();
			}

			//draw a circle around each touch point
			if (null != TouchManager)
			{
				SpriteBatch.Begin();

				//go though the points that are being touched
				TouchCollection touchCollection = TouchPanel.GetState();
				foreach (var touch in touchCollection)
				{
					if ((touch.State == TouchLocationState.Pressed) || (touch.State == TouchLocationState.Moved))
					{
						DrawHelper.Prim.Circle(touch.Position, 40.0f, new Color(1.0f, 1.0f, 1.0f, 0.25f));
					}
				}

				SpriteBatch.End();
			}
#endif
		}

		#endregion //Methods
	}
}