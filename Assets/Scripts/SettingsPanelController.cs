using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] public GameObject bottomSheet;
    [SerializeField] public Slider velocitySlider;
    [SerializeField] private TextMeshProUGUI velocityValueText;

    private bool isOpen = false;

    // void Start()
    // {
    //     bottomSheet.SetActive(false);

    //     // Set slider range
    //     velocitySlider.minValue = 1f;
    //     velocitySlider.maxValue = 20f;

    //     // Set default value (from Singleton instance)
    //     velocitySlider.value = SlingShotHandler.instance._shotForce;
    //     velocityValueText.text = $"Velocity: {velocitySlider.value:F1}";

    //     // Log current velocity
    //     Debug.Log($"Initial Velocity: {velocitySlider.value:F1}");

    //     // Attach listener
    //     velocitySlider.onValueChanged.AddListener(OnVelocityChanged);
    // }

    void Start()
    {
        bottomSheet.SetActive(false);

        velocitySlider.minValue = 1f;
        velocitySlider.maxValue = 10f;

        // Load saved velocity or use default (10f)
        float savedVelocity = PlayerPrefs.GetFloat("ShotForce", 5f);
        SlingShotHandler.instance._shotForce = savedVelocity;

        velocitySlider.value = savedVelocity;
        velocityValueText.text = $"{savedVelocity:F1}";

        Debug.Log($"Loaded Velocity: {savedVelocity:F1}");

        velocitySlider.onValueChanged.AddListener(OnVelocityChanged);
    }

    public void ToggleSettings()
    {
        isOpen = !isOpen;
        bottomSheet.SetActive(isOpen);
        Debug.Log($"Panel open: {isOpen}");
    }

    void OnVelocityChanged(float value)
    {
        SlingShotHandler.instance._shotForce = value;
        velocityValueText.text = $"{value:F1}";

        // Save the velocity
        PlayerPrefs.SetFloat("ShotForce", value);
        PlayerPrefs.Save();

        Debug.Log($"Velocity changed to: {value:F1}");
    }
}
