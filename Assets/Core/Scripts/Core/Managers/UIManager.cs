using UnityEngine;
using TMPro;

/// <summary>
/// UIManager:
/// Controla la UI del juego.
/// Por ahora solo muestra el score de monedas.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _lifeText;

    private int _coinCount = 0;
    private int _lifeCount = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddCoin(int amount)
    {
        _coinCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _coinText.text = _coinCount.ToString();
    }

    public void AddLife(int amount)
    {
        _lifeCount += amount;
        UpdateLifeUI();
    }

    private void UpdateLifeUI()
    {
        _lifeText.text = _lifeCount.ToString();
    }
}