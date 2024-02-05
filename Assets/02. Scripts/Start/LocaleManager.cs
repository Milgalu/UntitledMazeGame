using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;    

public class LocaleManager : MonoBehaviour
{
    bool isChanging;

    public void ChangeLocale(int index)
    {
        if (isChanging)
            return;

        isChanging = true;

        StartCoroutine(ChangeLocaleCoroutine(index));
        Debug.Log("Changing locale to " + LocalizationSettings.AvailableLocales.Locales[index].LocaleName);
    }

    IEnumerator ChangeLocaleCoroutine(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;
    }
}
