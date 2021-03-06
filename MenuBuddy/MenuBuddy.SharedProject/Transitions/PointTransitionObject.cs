using Microsoft.Xna.Framework;
using System;

namespace MenuBuddy
{
	/// <summary>
	/// This is a class that takes a start postion and transitions to the final position
	/// </summary>
	public class PointTransitionObject : BaseTransitionObject
	{
		#region Properties

		/// <summary>
		/// This is the location that the transition object will start from.
		/// </summary>
		public Vector2 StartPosition { get; set; }

		#endregion //Properties

		#region Methods

		public PointTransitionObject(Vector2 startPosition, IScreenTransition screenTransition = null) :
			base(screenTransition)
		{
			StartPosition = startPosition;
		}

		public override Point Position(IScreen screen, Rectangle rect)
		{
			var pos = Position(screen, rect.Location.ToVector2());
			return pos.ToPoint();
		}

		public override Vector2 Position(IScreen screen, Point pos)
		{
			return Position(screen, pos.ToVector2());
		}

		public override Vector2 Position(IScreen screen, Vector2 pos)
		{
			var screenTransition = GetScreenTransition(screen);

			//just return the end position if no transition stuff.
			Vector2 result = pos;
			if (screenTransition.TransitionPosition != 0.0f)
			{
				//get the transition offset
				var transitionOffset = (float)Math.Pow(screenTransition.TransitionPosition, 2.0);
				result = Vector2.Lerp(pos, StartPosition, transitionOffset);
			}

			LeftOrRight = result.X < pos.X;
			
			return result;
		}

		#endregion //Methods
	}
}