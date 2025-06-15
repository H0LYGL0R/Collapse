using Microsoft.Xna.Framework;
using System;

namespace Collapse.Code.Model.GameObjects
{
    public abstract class GameObjectModel
    {
        protected Rectangle? _rectangle;


        public Rectangle Rectangle
        {
            get
            {
                return _rectangle ?? throw new Exception("Прямоугольник не создан");
            }
            protected set
            { 
                _rectangle = value;
            }
        }
        public uint Width { get; protected set; } 
        public uint Height { get; protected set; }

        public event Action<Vector2> OnChangePosition;

        private Vector2? _position;
        public virtual Vector2 Position
        {
            get => Rectangle.Location.ToVector2();

            set
            {
                if (_rectangle is null)
                {
                    _position = value;
                    return;
                }
                
                Rectangle = new Rectangle((int)value.X, (int)value.Y, (int)Width, (int)Height);
                OnChangePosition?.Invoke(value);
            }
        }

        public GameObjectModel(uint width, uint height)
        {
            _rectangle = null;
            Height = height;
            Width = width;
        }

        public bool DoesCollidePoint(int xPoint, int yPoint) => Rectangle.Contains(xPoint, yPoint);

        public virtual void CreateRectangle(Vector2? position=null)
        {
            if (position is not null)
                _position = position;
            if (_position is null) throw new Exception("Положение для создаваемого прямоугольника не задано");
            Rectangle = new Rectangle((int)_position.Value.X, (int)_position.Value.Y, (int)Width, (int)Height);
        }

        public virtual void DestroyRectangle()
        {
            _rectangle = null;
            _position = null;
        }
    }
}