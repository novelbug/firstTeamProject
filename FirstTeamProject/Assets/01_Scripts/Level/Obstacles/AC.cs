using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AC : MonoBehaviour
{
    private void Awake()
    {
        transform.DOMove(transform.position + Vector3Int.down * 15, 0.5f).SetEase(Ease.InSine)
            .OnComplete(() => { Destroy(gameObject); });
    }
}
