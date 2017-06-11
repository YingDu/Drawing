namespace Drawing.Models
{
    public abstract class Decorator : IDrawable
    {
        public ShapeBase Shape { get; }

        public Decorator(ShapeBase shape)
        {
            Shape = shape;
        }

        public virtual void Draw()
        {
            Shape?.Draw();
        }
    }

    class OpacityDecorator : Decorator
    {
        public OpacityDecorator(ShapeBase shape) : base(shape)
        {
        }

        public override void Draw()
        {
            base.Draw();
            if (Shape != null && Shape.Instance !=null)
            {
                Shape.Instance.Opacity = 0.5; 
            }
        }
    }
}
