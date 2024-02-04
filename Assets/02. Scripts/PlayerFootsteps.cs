using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip[] jumpSounds;
    public AudioClip[] landSounds;
    public AudioClip[] walkSounds;
    public AudioClip[] runSounds;

    private float jumpCooldown = 1.5f; // 점프 쿨타임
    private float runCooldown = 0.3f;  // 뛰기 쿨타임
    private float walkCooldown = 0.5f; // 걷기 쿨타임

    private float jumpTimer;
    private float runTimer;
    private float walkTimer;

    private CapsuleCollider capsuleCollider;
    private bool wasGrounded = true;

    private PlayerController playerController;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        jumpTimer = 0f;
        runTimer = 0f;
        walkTimer = 0f;

        // PlayerController 스크립트를 가져옴
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // 각 동작의 쿨타임이 지난 경우에만 해당 동작을 감지
        if (jumpTimer <= 0f && Input.GetKeyDown(KeyCode.Space))
        {
            PlayRandomSound(jumpSounds);
            jumpTimer = jumpCooldown;
        }

        // 땅에 닿아 있는지 여부 확인
        bool isGrounded = CheckIfGrounded();

        if (isGrounded && !wasGrounded)
        {
            // 땅에서 점프할 때 소리 재생
            PlayRandomSound(landSounds);
        }

        if (isGrounded)
        {
            if (playerController.isSpeedBoostActive && runTimer <= 0f)
            {
                PlayRandomSound(runSounds);
                runTimer = runCooldown;
            }

            if (!playerController.isSpeedBoostActive && walkTimer <= 0f && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                PlayRandomSound(walkSounds);
                walkTimer = walkCooldown;
            }
        }

        // 각 동작의 쿨타임 감소
        jumpTimer -= Time.deltaTime;
        runTimer -= Time.deltaTime;
        walkTimer -= Time.deltaTime;

        wasGrounded = isGrounded;
    }

    private bool CheckIfGrounded()
    {
        // 캡슐 콜라이더 아래의 중심에서 캡슐 콜라이더의 반지름만큼의 거리만큼 아래 방향으로 Ray를 쏘아 땅과의 충돌을 감지
        float rayLength = capsuleCollider.bounds.extents.y + 0.1f;
        return Physics.Raycast(transform.position, Vector3.down, rayLength);
    }

    private void PlayRandomSound(AudioClip[] soundArray)
    {
        if (soundArray.Length > 0)
        {
            // 무작위로 소리 선택
            AudioClip randomSound = soundArray[Random.Range(0, soundArray.Length)];

            // 선택한 소리 재생
            footstepAudioSource.PlayOneShot(randomSound);
        }
    }
}
