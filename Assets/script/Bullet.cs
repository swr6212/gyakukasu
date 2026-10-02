using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Collider objectCollider;
    private Rigidbody rb;

    void Start()
    {
        GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 1); //弾の色を黒にする
        objectCollider = GetComponent<SphereCollider>();
        objectCollider.isTrigger = false; //Triggerとして扱う
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; //重力を無効にする
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block")) //タグがBlockのオブジェクトと衝突した場合
        {
            Destroy(this.gameObject); //弾を消す
        }

        if (collision.gameObject.CompareTag("Enemy")) //タグがEnemyのオブジェクトと衝突した場合
        {
            
            
         
            Destroy(this.gameObject); //弾を消す
        }
    }
}