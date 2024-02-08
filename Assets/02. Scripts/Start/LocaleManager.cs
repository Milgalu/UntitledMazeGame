using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleManager : MonoBehaviour
{
    bool isChanging;
    public Font englishFont;
    public Font koreanFont;
    public Font japaneseFont;

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
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        // 언어 변경 시 폰트도 변경
        SwitchFont(index);

        isChanging = false;
    }

    void Start()
    {
        SwitchFont(0);
    }

    void SwitchFont(int language)
    {
        Text[] texts = FindObjectsOfType<Text>(); // 해당 씬에서 모든 Text 요소 찾기
        // 언어에 따라 폰트 변경
        foreach (Text text in texts)
        {
            switch (language)
            {
                case 0:
                    text.font = koreanFont;
                    break;
                case 1:
                    text.font = englishFont;
                    break;
                case 2:
                    text.font = japaneseFont;
                    break;
                // 추가 언어에 대한 케이스를 필요에 따라 추가할 수 있습니다.
                default:
                    text.font = englishFont; // 기본값으로 영어 폰트를 사용
                    break;
            }
        }
    }
}
