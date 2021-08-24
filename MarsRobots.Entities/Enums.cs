using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MarsRobots.Entities.Enums
{
	/// <summary>
	/// Enum for available robot actions.
	/// </summary>
	public enum RobotAction
	{
		/// <summary>
		/// Rotates 90º left.
		/// </summary>
		[Description("Left")]
		Left,
		/// <summary>
		/// Rotates 90º right.
		/// </summary>
		[Description("Right")]
		Right,
		/// <summary>
		/// Goes forward one unit towards current direction.
		/// </summary>
		[Description("Forward")]
		Forward
	}

	/// <summary>
	/// Cardinal directions enum.
	/// </summary>
	public enum CardinalDirection
	{
		// Commented values of enum would increase robot rotation resolution.

		[Description("Morth")]
		N,
		//[Description("MorthEast")]
		//NE,
		[Description("East")]
		E,
		//[Description("SouthEast")]
		//SE,
		[Description("South")]
		S,
		//[Description("SouthWest")]
		//SW,
		[Description("West")]
		W
		//[Description("MorthWest")]
		//NW
	}
}
