using Godot;
using System;

namespace SummerFediverseJam
{
    public class DialogArea : Area2D
    {
        protected bool DisableCheck = false;
        private bool isPlayerOverlapping { get; set; }
        protected bool IsPlayerOverlapping { 
            get
            {
                return isPlayerOverlapping;
            }
            set
            {
                if (value != isPlayerOverlapping)
                {
                    NotifyOverlapChange(value);
                }
                isPlayerOverlapping = value;
            }
        }
        protected Player __player { get; set; }
        private RectangleShape2D __collisionShape { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            __collisionShape = (RectangleShape2D) GetNode<CollisionShape2D>("CollisionShape2D").Shape;
            var parent = GetParent();
            while (__player == null)
            {
                if (parent.HasNode("Player"))
                {
                    __player = parent.GetNode<Player>("Player");
                }
                parent = parent.GetParent();
            }
        }

        public virtual void NotifyOverlapChange(bool overlap)
        {

        }

        public virtual DialogText[] GetDialog(Player player)
        {
            return new DialogText[0];
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (DisableCheck)
                return;
            // 3 is the magic number
            bool isLessThanX = __player.GlobalPosition.x < GlobalPosition.x + __collisionShape.Extents.x * 3;
            bool isGreaterThanX = __player.GlobalPosition.x > GlobalPosition.x - __collisionShape.Extents.x * 3;
            bool isLessThanY = __player.GlobalPosition.y < GlobalPosition.y + __collisionShape.Extents.y * 3;
            bool isGreaterThanY = __player.GlobalPosition.y > GlobalPosition.y - __collisionShape.Extents.y * 3;
            IsPlayerOverlapping = isLessThanX && isGreaterThanX && isLessThanY && isGreaterThanY;
        }
    }
}