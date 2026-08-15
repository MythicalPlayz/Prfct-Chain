using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(5)]
public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // إيقاف حركة الفيزياء تماماً في البداية
        }
    }

    private void Start()
    {
        StartCoroutine(OnStart());
    }

    IEnumerator OnStart()
    {
        // انتظر ثانية واحدة (حسب كودك الأصلي)
        yield return new WaitForSeconds(1f);

        // انتظر هنا ولن يكمل الكود إلا لما تدوس Space
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        // تفعيل الفيزياء مرة أخرى لتبدأ الكورة في الحركة
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        GameManager.Instance.SetBall(gameObject);
    }
}