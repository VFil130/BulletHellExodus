using System.Collections;
using UnityEngine;
using DG.Tweening;
public class Swing : Melee
{
    private void Start()
    {
        Debug.Log("Melee Start вызван");
        Debug.Log("Collider включен: " + GetComponent<Collider2D>().enabled);
        Debug.Log("Is Trigger: " + GetComponent<Collider2D>().isTrigger);
        Debug.Log("Есть Rigidbody2D: " + (GetComponent<Rigidbody2D>() != null));
        StartCoroutine(Swinging());

    }
}
