using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Image icon;

    public TextMeshProUGUI Text => text;

    public Image Icon => icon;
}
