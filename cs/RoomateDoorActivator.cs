using Godot;
using System;

namespace SummerFediverseJam {
	public class RoomateDoorActivator : Area2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		private DeadPlant __plant;
		private CollisionShape2D __collisionShape;
		private CircleShape2D __circleShape;
		private Player __player;
		private AnimatedSprite __roommateDoor;
		private KitchenMask __roommateMask;
		private CollisionShape2D __roommateDoorCollider;
		private AudioStreamPlayer __audioPlayer;
		private bool IsActivated;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			__plant = GetParent().GetNode<DeadPlant>("Plant After It Has Drowned");
			__collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
			__circleShape = (CircleShape2D)__collisionShape.Shape;
			__player = GetParent().GetNode<Player>("Player");
			__roommateDoor = GetParent().GetNode<AnimatedSprite>("Roomate Door");
			__roommateMask = GetParent().GetNode<KitchenMask>("Roomate Room/Roommate mask");
			__roommateDoorCollider = GetParent().GetNode<CollisionShape2D>("Roomate Door Collider/CollisionShape2D");
			__audioPlayer = GetNode<AudioStreamPlayer>("kerchunk");
		}
		private void Activate()
		{
			__player.NewDialog(new[] {
				new DialogText
				{
					Text = "[i]*ker-chunk*[/i]",
					OnDisplay = () =>
					{
						__audioPlayer.Play();
					},
					AfterDequeue = () =>
					{
						__player.stats.UsedRoommatesPlantToOpenDoor = true;
						__player.PlayAchievedJingle();
						__roommateMask.ExtraCondition = true;
						__roommateDoor.Play("opendoor");
						__roommateDoor.ZIndex = 2;
						__roommateDoorCollider.Disabled = true;
					}
				}
			});
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			if (!IsActivated)
			{
				var isWithinX = __plant.Position.x > Position.x - __circleShape.Radius && __plant.Position.x < Position.x + __circleShape.Radius;
				var isWithinY = __plant.Position.y > Position.y - __circleShape.Radius && __plant.Position.y < Position.y + __circleShape.Radius;
				if (isWithinY && isWithinX)
				{
					Activate();
					IsActivated = true;
				}
			}
		}
	}
}
