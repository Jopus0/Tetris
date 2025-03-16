using UnityEngine;
using UnityEngine.UI;

public class BlockPanel : MonoBehaviour
{
    [SerializeField] private Image _figureImage;

    public void SetFigureImage(Sprite sprite, Color color)
    {
        _figureImage.sprite = sprite;
        _figureImage.color = color;
    }
}
