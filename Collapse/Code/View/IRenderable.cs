
namespace Collapse.Code.View
{
    public interface IRenderable
    {
        RenderLayer Layer { get; }
        void Draw();
    }
}