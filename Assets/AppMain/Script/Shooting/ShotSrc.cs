using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ShotSrc : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject Smog;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float bulletPower = 500f;
    [SerializeField] Slider FireBar= null;
    [System.Serializable]
    public class Status
    {
        // 体力
        public float Hp = 3;
    }
    // 基本ステータス
    [SerializeField] Status DefaultStatus = new Status();
    // 現在のステータス
    public Status CurrentStatus = new Status();

    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private Vector3 minBounds; // 移動範囲の最小値
    [SerializeField] private Vector3 maxBounds; // 移動範囲の最大値

    private bool isShooting = false;
    // Start is called before the first frame update
    void Start()
    {
        FireBar = GameObject.Find("Slider").GetComponent<Slider>();
        // スライダーの初期設定
        FireBar.maxValue = DefaultStatus.Hp;
        FireBar.value = CurrentStatus.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShooting();
        if (isShooting)
        {
            DecreaseFireBarOverTime();
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical ,0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // 現在の位置を取得
        Vector3 clampedPosition = transform.position;

        // X軸の範囲制限
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        // y軸の範囲制限
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);
        // z軸の範囲制限
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.z, maxBounds.z);

        // 制限された位置を適用
        transform.position = clampedPosition;
    }

    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isShooting = !isShooting;
        }

        if (isShooting && CurrentStatus.Hp > 0)
        {
            // 弾丸の発射位置をmuzzleの現在位置からy座標を+10した位置に設定
            Vector3 shootPosition = new Vector3(muzzle.position.x, muzzle.position.y + 0.5f, muzzle.position.z);
            Quaternion shootRotation = muzzle.rotation;

            var bulletInstance = Instantiate<GameObject>(bulletPrefab, shootPosition, shootRotation);
            bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.up * bulletPower);
            Destroy(bulletInstance, 5f);
        }

        if (CurrentStatus.Hp <= 0)
        {
            Fire.SetActive(false);
            Smog.SetActive(true);
            SceneManager.LoadScene("ConversationScene5");
        }
    }

    private void DecreaseFireBarOverTime()
    {
        if (CurrentStatus.Hp > 0)
        {
            CurrentStatus.Hp -= Time.deltaTime * 1.5f; // 時間経過で体力を減少
            FireBar.value = CurrentStatus.Hp;
        }
        else
        {
            Fire.SetActive(false);
            Smog.SetActive(true);
            SceneManager.LoadScene("ConversationScene5");
        }
    }
}
