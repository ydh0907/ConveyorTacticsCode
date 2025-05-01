using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public Animal Owner { get; protected set; }
    public bool OnSkill { get; protected set; } = false;
    public abstract int Cost { get; }
    public virtual void SetOwner(Animal owner) => Owner = owner;
    public abstract bool CanUse(Block block);
    public async UniTask Use(Block block)
    {
        if (SystemManager.Instance.activeBar.CheckMinusActiveValue())
        {
            SystemManager.Instance.activeBar.CompleteBarValue();
            await OnUse(block);
        }
    }
    protected abstract UniTask OnUse(Block block);
    public abstract void UpdateData(Block block);
    public virtual void Select()
    {
        Owner.ShowMovable(false);
        SystemManager.Instance.activeBar.CheckMinusBar(Cost);
    }
    public virtual void Unselect()
    {
        Owner.ShowMovable(true);
        SystemManager.Instance.activeBar.CancelBarValue();
    }
}
