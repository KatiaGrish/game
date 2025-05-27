using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // Ссылка на TextMeshProUGUI элемент

    // Обновляем текст с текущим количеством монет
    public void UpdateMoneyText(int coins)
    {
        if (moneyText != null)
        {
            moneyText.text = coins.ToString();
        }
    }
}