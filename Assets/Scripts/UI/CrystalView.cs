using TMPro;
using UnityEngine;

public class CrystalView : MonoBehaviour
{
    [SerializeField] private CrystalCounter _crystalCounter;
    [SerializeField] private TMP_Text _crystalText;

    private void OnEnable()
    {
        Render();
        _crystalCounter.AmountUpdated += Render;
    }

    private void OnDisable()
    {
        _crystalCounter.AmountUpdated -= Render;
    }

    private void Render()
    {
        _crystalText.text = $"{_crystalCounter.Amount}";
    }
}