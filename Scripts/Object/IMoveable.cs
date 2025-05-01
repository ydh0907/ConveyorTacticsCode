using Cysharp.Threading.Tasks;

public interface IMoveable // ??? ????
{
    public bool IsBlockMove { get; set; }
    public abstract bool CanMove(Block block);
    public abstract UniTask Move(Block block);
}
