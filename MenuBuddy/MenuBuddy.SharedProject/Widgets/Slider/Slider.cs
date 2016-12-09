namespace MenuBuddy
{
	public class Slider : BaseSlider<float>
	{
		#region Properties

		public override float SliderPosition
		{
			get
			{
				return HandlePosition;
			}
			set
			{
				HandlePosition = value;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor
		/// </summary>
		public Slider()
		{
		}

		public Slider(Slider inst) : base(inst)
		{
		}

		/// <summary>
		/// Get a deep copy of this item
		/// </summary>
		/// <returns></returns>
		public override IScreenItem DeepCopy()
		{
			return new Slider(this);
		}

		#endregion //Methods
	}
}