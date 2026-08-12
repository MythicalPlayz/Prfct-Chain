using System.Collections;
using UnityEngine;
[DefaultExecutionOrder(5)]
public class Ball : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(OnStart());
    }

    IEnumerator OnStart()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SetBall(gameObject);
    }
}
