using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] private RectTransform icon;

    private void Start()
    {
        var tween = icon.DORotate(Vector3.forward * 360f, 1f, RotateMode.LocalAxisAdd);
        tween.SetLoops(-1);
    }

    public void SetEnable(bool enable)
    {
        icon.GetComponent<Image>().color = new Color(1f, 1f, 1f, enable ? 1f : 0f);
    }

    public void SettleOnTop()
    {
        GetComponent<RectTransform>().SetAsLastSibling();
    }
}