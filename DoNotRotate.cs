using Godot;
using System;

namespace SummerFediverseJam
{

	public class DoNotRotate : RigidBody2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}
		public override void _IntegrateForces(Physics2DDirectBodyState state)
		{
			base._IntegrateForces(state);
			// Translation = _initialPosition;
			// Discard rotation around any other access than Y
			Rotation = 0;
		}
	}

}
