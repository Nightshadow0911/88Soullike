using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTesTT : MonoBehaviour
{
    public float shakeDuration = 0.5f;      // 쉐이크 지속 시간
    public float shakeMagnitude = 0.1f;     // 쉐이크 진폭
    public float shakeFadeOutTime = 0.5f;   // 쉐이크 페이드아웃 시간

    private Vector3 originalPosition;       // 원래 카메라 위치
    public float knockbackForce = 5f;         // 넉백 힘
    public float knockbackDuration = 0.5f;    // 넉백 지속 시간

    public Rigidbody2D playerRigidbody;
    public ParticleSystem particleSystem;

    public float flashDuration = 0.1f;       // 번쩍임 지속 시간
    public Color flashColor = Color.white;   // 번쩍임 색상

    public SpriteRenderer spriteRenderer;

    public float zoomDuration = 0.3f;   // 줌인 지속 시간
    public float targetZoom = 2f;       // 목표 줌 레벨
    public AnimationCurve zoomCurve;    // 줌인 곡선

    private Camera mainCamera;

    public float stopDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region CameraShake
    public void StartShake()
    {
        originalPosition = Camera.main.transform.position;
        StartCoroutine(ShakeCoroutine());
    }
    // 쉐이크 코루틴
    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // 랜덤한 쉐이크 위치 계산
            Vector3 shakePosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // 카메라 위치 조정
            Camera.main.transform.position = shakePosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        // 쉐이크 페이드아웃
        float fadeElapsedTime = 0f;
        while (fadeElapsedTime < shakeFadeOutTime)
        {
            // 점점 원래 위치로 복구
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, originalPosition, fadeElapsedTime / shakeFadeOutTime);

            fadeElapsedTime += Time.deltaTime;

            yield return null;
        }
        // 최종 위치 복구
        Camera.main.transform.position = originalPosition;
    }
    #endregion
    #region Knockback
    public void Knockback()
    {
        StartCoroutine(KnockbackCoroutine());
    }

    // 넉백 코루틴
    private IEnumerator KnockbackCoroutine()
    {
        // 넉백 방향과 힘 계산
        Vector2 knockbackDirection = Vector2.left;
        Vector2 knockbackForceVector = knockbackDirection * knockbackForce;

        // 넉백 힘 적용
        playerRigidbody.velocity = knockbackForceVector;

        // 넉백 지속 시간만큼 대기
        yield return new WaitForSeconds(knockbackDuration);

        // 넉백 힘 초기화
        playerRigidbody.velocity = Vector2.zero;
    }
    #endregion
    #region Effect
    public void StartEffect()
    {
        StartCoroutine(PlayParticleEffectCoroutine());
    }
    private IEnumerator PlayParticleEffectCoroutine()
    {
        // 파티클 시스템 재생
        particleSystem.Play();
        // 파티클 재생 시간만큼 대기
        yield return new WaitForSeconds(particleSystem.main.duration);

        // 파티클 시스템 정지
        particleSystem.Stop();
    }
    #endregion
    #region ScreenFlash
    // 화면 번쩍임
    public void FlashScreen()
    {
        StartCoroutine(FlashCoroutine());
    }

    // 번쩍임 코루틴
    private IEnumerator FlashCoroutine()
    {
        // 원래 색상 저장
        Color originalColor = spriteRenderer.color;

        // 번쩍임 색상 적용
        spriteRenderer.color = flashColor;

        // 번쩍임 지속 시간만큼 대기
        yield return new WaitForSeconds(flashDuration);

        // 원래 색상 복구
        spriteRenderer.color = originalColor;
    }
    #endregion
    #region ZoomIn
    public void ZoomIn()
    {
        StartCoroutine(ZoomInCoroutine());
    }
    private IEnumerator ZoomInCoroutine()
    {
        float startTime = Time.time;
        float initialZoom = mainCamera.orthographicSize;

        while (Time.time - startTime < zoomDuration)
        {
            float t = (Time.time - startTime) / zoomDuration;
            float newZoom = Mathf.Lerp(initialZoom, targetZoom, zoomCurve.Evaluate(t));

            mainCamera.orthographicSize = newZoom;

            yield return null;
        }
        mainCamera.orthographicSize = targetZoom;
    }
    #endregion
    #region TimeStop
    public void StopPlayerTime()
    {
        StartCoroutine(StopPlayerTimeCoroutine());
    }
    private IEnumerator StopPlayerTimeCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(stopDuration);
        Time.timeScale = originalTimeScale;
    }
    #endregion

}
