using Game.Resources;

namespace Game.Components
{
    public interface IItemSink
    {
        bool AcceptItem(Direction fromSide, Item stack);
    }
}