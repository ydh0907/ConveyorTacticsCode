using Cysharp.Threading.Tasks;

public interface ISetable
{
    public UniTask Set(Block block);
}
