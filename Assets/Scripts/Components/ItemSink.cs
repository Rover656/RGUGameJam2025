using Game.Resources;

namespace Game.Components
{
    public class ItemSink : GridMonoBehaviour, IItemSink
    {
        public bool AcceptItem(Direction fromSide, Item stack)
        {
            return true;
        }
    }
}