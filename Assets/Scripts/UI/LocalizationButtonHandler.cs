using UnityEngine;

public class LocalizationButtonHandler : MonoBehaviour
{
    [SerializeField] private LocalizationLanguageEnum _localizationLanguage;

    public void OnClick()
    {
        if (LocalizationSystem.instance == null)
        {
            Debug.LogError("LocalizationSystem instance not found!");
            return;
        }
        LocalizationSystem.instance.LoadLanguage(_localizationLanguage);
    }
}