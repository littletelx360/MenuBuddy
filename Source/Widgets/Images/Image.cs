using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuBuddy
{
	/// <summary>
	/// An image that is displayed onteh screen
	/// </summary>
	public class Image : Widget, IImage
	{
		#region Properties

		public Texture2D Texture
		{
			get { return Style.Texture; }
			set
			{
				Style.Texture = value;
				Rect = new Rectangle(Rect.X, Rect.Y, Style.Texture.Width, Style.Texture.Height);
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor!
		/// </summary>
		/// <param name="style"></param>
		public Image(StyleSheet style)
			: base(style)
		{
		}

		/// <summary>
		/// constructor!
		/// </summary>
		public Image(StyleSheet style, Texture2D texture)
			: this(style)
		{
			Texture = texture;
		}

		public override void Update(IScreen screen, GameClock gameTime)
		{
		}

		public override void Draw(IScreen screen, GameClock gameTime)
		{
			if (!ShouldDraw(screen))
			{
				return;
			}

			//get the transition color
			var color = screen.Transition.AlphaColor(Color.White);

			//Get the transition location
			var pos = DrawPosition(screen);

			//draw the item with all the correct parameters
			screen.ScreenManager.SpriteBatch.Draw(Texture, new Rectangle((int)pos.X, (int)pos.Y, Rect.Width, Rect.Height), color);
		}

		#endregion //Methods
	}
}