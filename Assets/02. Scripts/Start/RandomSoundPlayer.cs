using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] soundClips; // 사운드 클립 배열
    public float minDelay = 1f; // 최소 딜레이
    public float maxDelay = 5f; // 최대 딜레이

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 최초에 사운드 재생 시작
        PlayRandomSound();
    }

    void PlayRandomSound()
    {
        // 랜덤으로 사운드 클립 선택
        AudioClip randomClip = soundClips[Random.Range(0, soundClips.Length)];

        // 선택된 클립을 AudioSource에 설정
        audioSource.clip = randomClip;

        // 사운드 재생
        audioSource.Play();

        // 다음 사운드 재생을 위한 랜덤 딜레이 설정
        float delay = Random.Range(minDelay, maxDelay);
        Invoke("PlayRandomSound", delay);
    }
}
