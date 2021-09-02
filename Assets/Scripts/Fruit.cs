using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private float _spriteSize = 0.3f;

    public void SetupFruit(Sprite _sprite)
    {
        GetComponent<SpriteRenderer>().sprite = _sprite;
    }
}
