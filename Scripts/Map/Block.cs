using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Block Actions")]
    public ISetable Current { get; private set; }
    public Item Item { get; private set; }

    private int blockIndex = 0;
    public int BlockIndex
    {
        get => blockIndex;
        set => blockIndex = value;
    }
    public Vector2Int Idx => MapManager.Instance.GetIdx(this);
    public Action destroyNext = null;

    [Header("Animated Block")]
    [SerializeField] private new MeshRenderer renderer;
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material selectedMat;
    [SerializeField] private Material showMat;

    private CancellationTokenSource Stop = new();

    private bool show = false;
    public bool IsBlocked { get; private set; } = false;
    public Material DefaultMat => defaultMat;

    private void Awake()
    {
        defaultMat = renderer.material;
    }

    public void SetBlock(ISetable block)
    {
        Current = block;
    }

    public void SetItem(Item item)
    {
        Item = item;
    }

    public void Select()    
    {
        if (IsBlocked) return;
        Stop?.Cancel();
        renderer.material = selectedMat;
        MoveLocalY(0.5f, 0.2f).Forget();
    }

    public void Unselect()
    {
        if (IsBlocked) return;
        Stop?.Cancel();
        if (show)
        {
            renderer.material = showMat;
            MoveLocalY(0.25f, 0.2f).Forget();
        }
        else
        {
            renderer.material = defaultMat;
            MoveLocalY(0.0f, 0.2f).Forget();
        }
    }

    public void SelectShow()
    {
        if (IsBlocked) return;
        Stop?.Cancel();
        show = true;
        renderer.material = showMat;
        MoveLocalY(0.25f, 0.2f).Forget();
    }

    public void SelectUnshow()
    {
        if (IsBlocked) return;
        Stop?.Cancel();
        show = false;
        renderer.material = defaultMat;
        MoveLocalY(0.0f, 0.2f).Forget();
    }

    private async UniTask MoveLocalY(float y, float time)
    {
        Stop = new();
        await transform.DOLocalMoveY(y, time).ToUniTask(cancellationToken: Stop.Token);
    }
    
    public void ChangeMaterial(Material material)
    {
        if (renderer is null)
        {
            Debug.LogError("Renderer is null");
            return;
        }
        if (material is null)
        {
            Debug.LogError("Material is null");
            return;
        }
        if (renderer?.material is null)
        {
            Debug.LogError("Renderer material is null");
            return;
        }

        if (ReferenceEquals(renderer, null))
        {
            Debug.LogError("Renderer is null!!!!!!!");
            return;
        }
        Debug.Assert(renderer != null, "<color=black>Renderer is null</color>");
            renderer.material = material;
    }
    
    public void SetBlocked(bool value)
    {
        IsBlocked = value;
    }
}
