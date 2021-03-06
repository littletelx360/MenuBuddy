using InputHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ResolutionBuddy;
using System.Threading.Tasks;

namespace MenuBuddy
{
	/// <summary>
	/// THis is a menu entry that pops up from the bottom of a window and says "Continue"
	/// </summary>
	public class CancelButton : RelativeLayoutButton
	{
		#region Properties

		private string IconTextureName { get; set; }

		public Image CancelIcon { get; private set; }

		private int? CustomSize;

		#endregion //Properties

		#region Methods

		public CancelButton(string iconTextureName = "")
		{
			ClickedSound = StyleSheet.CancelButtonSoundResource;
			IconTextureName = !string.IsNullOrEmpty(iconTextureName) ? iconTextureName : StyleSheet.CancelButtonImageResource;
		}

		public CancelButton(int? customSize) : this()
		{
			CustomSize = customSize;
		}

		public override async Task LoadContent(IScreen screen)
		{
			await base.LoadContent(screen);

			//load the icon
			TransitionObject = new WipeTransitionObject(TransitionWipeType.PopRight);
			HasBackground = false;
			HasOutline = StyleSheet.CancelButtonHasOutline;
			Horizontal = HorizontalAlignment.Right;
			Vertical = VerticalAlignment.Top;
			DrawWhenInactive = false;

			CancelIcon = new Image(screen.Content.Load<Texture2D>(IconTextureName))
			{
				Vertical = VerticalAlignment.Top,
				Horizontal = HorizontalAlignment.Right,
				TransitionObject = new WipeTransitionObject(TransitionWipeType.PopRight),
			};
			AddItem(CancelIcon);

			//set the size to the texture size
			Vector2 size = Vector2.Zero;
			if (CustomSize.HasValue)
			{
				size = new Vector2(CustomSize.Value);
				CancelIcon.FillRect = true;
				CancelIcon.Size = size;
			}
			else if (StyleSheet.CancelButtonSize.HasValue)
			{
				size = new Vector2(StyleSheet.CancelButtonSize.Value);
				CancelIcon.FillRect = true;
				CancelIcon.Size = size;
			}
			else
			{
				size = new Vector2(CancelIcon.Texture.Bounds.Width, CancelIcon.Texture.Bounds.Height);
			}

			var relLayout = Layout as RelativeLayout;
			relLayout.Size = size;
			Size = size;

			Position = new Point(Resolution.TitleSafeArea.Right, Resolution.TitleSafeArea.Top) + StyleSheet.CancelButtonOffset;

			//Exit the screen when this button is selected
			OnClick += ((object obj, ClickEventArgs e) =>
			{
				screen.ExitScreen();
			});
		}

		#endregion //Methods
	}
}