using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    public static int score = 0;
    [SerializeField] Text Score;

    public AudioClip sound1;
    AudioSource audioSource;

    public static PlayerController instance;

    //イベントパネル
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panel2;
    [SerializeField] GameObject panel3;
    public Transform target; // 近づくべきオブジェクトのTransform
    public Transform target3; // 近づくべきオブジェクトのTransform
    private events events1;
    private events2 events3;
    [SerializeField] GameObject bar;
    private bool isPanel3ManuallyHidden = false;


    [SerializeField] GameObject attackHit = null;
    [SerializeField] float jumpPower =20f;
    [SerializeField] ColliderCallReceiver footColliderCall = null;
    [SerializeField] GameObject touchMarker=null;
    [SerializeField] PlayerCameraController cameraController = null;
    // 攻撃ヒットオブジェクトのColliderCall
    [SerializeField] ColliderCallReceiver attackHitCall = null;
    // 自身のコライダー
    [SerializeField] Collider myCollider = null;
    // 攻撃を受けた時のパーフェクトプレハブ
    [SerializeField] GameObject hitParticlePrefab = null;
    // パーティクルオブジェクト保管リスト
    List<GameObject> particleObjectList = new List<GameObject>();
    [SerializeField] Slider hpBar= null;
    [SerializeField] ColliderCallReceiver aroundColliderCall = null;
    Animator animator = null;
    Rigidbody rigid = null;
    bool isAttack = false;
    bool isGround = false;
    [System.Serializable]


    public class Status
    {
        // 体力
        public int Hp = 10;
        // 攻撃力
        public int Power = 1;
    }
    // 基本ステータス
    [SerializeField] Status DefaultStatus = new Status();
    // 現在のステータス
    public Status CurrentStatus = new Status();

    
    // PCキー横方向入力
    float horizontalKeyInput = 0;
    // PCキー縦方向入力
    float verticalKeyInput = 0;
    // ゲームオーバー時イベント
    public UnityEvent GameOverEvent = new UnityEvent();
    // 開始位置
    Vector3 startPosition = new Vector3();
    // 開始角度
    Quaternion startRotation = new Quaternion();

    int hindranceDamage = 2;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        events1 = events.go;
        events3 = events2.go;
        animator = GetComponent<Animator>();
        attackHit.SetActive(false);
        rigid = GetComponent<Rigidbody>();
        footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
        footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
        // 攻撃判定用コライダーイベント登録
        attackHitCall.TriggerEnterEvent.AddListener(OnAttackHitTriggerEnter);
        // 障害物ダメージコライダーイベント
        // aroundColliderCall.TriggerStayEvent.AddListener( OnHindranceTriggerStay );
        // 現在のステータスの初期化
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        // 開始時の一回転を保管
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;

        // スライダーの初期設定
        hpBar.maxValue = DefaultStatus.Hp;
        hpBar.value = CurrentStatus.Hp;
    }

    bool isTouch = false;
    // 左半分タッチスタート位置
    Vector2 LeftStartTouch = new Vector2();
    // 左半分タッチ入力
    Vector2 LeftTouchInput = new Vector2();

    // Update is called once per frame
    void Update()
    {

        Score.text = string.Format("{0} Pt", score);

        // カメラをプレイヤーに向ける
        cameraController.UpdateCameraLook(this.transform);
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // スマホタッチ操作
            // タッチしている指の数が0より多い
            if(Input.touchCount > 0)
            {
                isTouch = true;
                // タッチ情報をすべて取得
                Touch[] touches = Input.touches;
                // 全部のタッチを繰り返して判定
                foreach(var touch in touches)
                {
                    bool isLeftTouch=false;
                    bool isRightTouch=false;
                    // タッチ位置のX軸方向がスクリーンの左側
                    if(touch.position.x>0 && touch.position.x<Screen.width/2)
                    {
                        isLeftTouch=true;
                    }
                    // タッチ位置のX軸方向がスクリーンの右側
                    else if(touch.position.x>Screen.width/2 && touch.position.x <Screen.width)
                    {
                        isRightTouch=true;;
                    }
                    // 左タッチ
                    if(isLeftTouch==true)
                    {
                        if(touch.phase == TouchPhase.Began)
                        {
                            Debug.Log("タッチ開始");
                            // 開始位置を保管
                            LeftStartTouch=touch.position;
                            // 開始時にマーカーを表示
                            touchMarker.SetActive(true);
                            Vector3 touchPosition = touch.position;
                            touchPosition.z=1f;
                            Vector3 markerPosition = Camera.main.ScreenToViewportPoint(touchPosition);
                            touchMarker.transform.position = markerPosition;
                            
                        }
                        else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                        {
                            Debug.Log("タッチ中");
                            // 現在地を保管
                            Vector2 position=touch.position;
                            // 移動用の方向を保管
                            LeftTouchInput=position-LeftStartTouch;
                        }
                        else if(touch.phase == TouchPhase.Ended)
                        {
                            Debug.Log("タッチ終了");
                            LeftTouchInput = Vector2.zero;
                            // マーカーを非表示
                            touchMarker.gameObject.SetActive(false);
                        }
                    }
                    // 右タッチ
                    if(isRightTouch==true)
                    {
                        // 右半分をタッチした時の処理
                        cameraController.UpdateRightTouch(touch);
                    }
                }
            }
            else
            {
                isTouch=false;
            }
        }
        else
        {// PCキー入力取得
            horizontalKeyInput = Input.GetAxis("Horizontal");
            verticalKeyInput = Input.GetAxis("Vertical");
        }
        // プレイヤーの向きを調整
        bool isKeyInput = (horizontalKeyInput != 0|| verticalKeyInput !=0 || LeftTouchInput !=Vector2.zero);
        if(isKeyInput == true && isAttack==false)
        {
            bool currentIsRun=animator.GetBool("isRun");
            if(currentIsRun == false) animator.SetBool("isRun",true);
            Vector3 dir = rigid.velocity.normalized;
            dir.y=0;
            this.transform.forward = dir;
            Debug.Log("Run");
        }
        else
            {
                bool currentIsRun = animator.GetBool("isRun");
                if(currentIsRun == true)animator.SetBool("isRun",false);
            }

        if (events1 == events.go)
        {
            if (Vector3.Distance(transform.position, target.position) < 2f)
            {
                events1 = events.stop;
                // 近づいたときの処理を記述する
                panel.SetActive(true);

            }
        }
        else if (events1 == events.stop)
        {
            if (Vector3.Distance(transform.position, target.position) > 2f)
            {
                events1 = events.go;
            }
        }



        if (events3 == events2.go && isPanel3ManuallyHidden == false)
        {
            if (Vector3.Distance(transform.position, target3.position) < 1f)
            {
                panel3.SetActive(true);
            }
        }
        else if (events3 == events2.stop)
        {
            if (Vector3.Distance(transform.position, target3.position) > 1f)
            {
                events3 = events2.go;
                isPanel3ManuallyHidden = false; // 再度距離が離れたら表示を許可
            }
        }

    }



    void FixedUpdate()
    {
        if(isAttack == false)
        {
            Vector3 input = new Vector3();
            Vector3 move = new Vector3();
            if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                input = new Vector3(LeftTouchInput.x,0,LeftTouchInput.y);
                move = input.normalized*2f;
            }
            else
            {
                input = new Vector3(horizontalKeyInput,0,verticalKeyInput);
                move = input.normalized * 2f;
            }
            
            Vector3 cameraMove = Camera.main.gameObject.transform.rotation * move;
            cameraMove.y=0;
            Vector3 currentRigidVelocity = rigid.velocity;
            currentRigidVelocity.y=0;

            rigid.AddForce(cameraMove-rigid.velocity,ForceMode.VelocityChange);
        }
        cameraController.FixedUpdateCameraPosition(this.transform);
    }
    
    public void OnAttackButtonClicked()
    {
        if(isAttack == false)
        {
            animator.SetTrigger("isAttack");
            isAttack = true;
        }
    }
    public void OnJumpButtonClicked()
    {
        if(isGround == true)
        {
            rigid.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
            Debug.Log("ジャンプボタンを押した");
        }
    }
    void OnFootTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Ground")
        {
            if(isGround == false) isGround =true;
            if(animator.GetBool("isGround")==false)animator.SetBool("isGround",true);
        }
    }
    void OnFootTriggerExit(Collider col)
    {
        if( col.gameObject.tag == "Ground")
        {
            isGround = false;
            animator.SetBool("isGround",false);
            Debug.Log("OnFootTriggerExit");
        }
    }
    void Anim_AttackHit()
    {
        Debug.Log("Hit");
        attackHit.SetActive(true);
    }
    void Anim_AttackEnd()
    {
        Debug.Log("End");
        attackHit.SetActive(false);
        isAttack = false;
    }
    void OnAttackHitTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Danger")
        {
            var enemy = col.gameObject.GetComponent<Danger>();
            enemy?.OnAttackHit(CurrentStatus.Power,this.transform.position);
            attackHit.SetActive(false);
        }
    }
    public void OnEnemyAttackHit(int damage,Vector3 attackPosition)
    {
        CurrentStatus.Hp -= damage;
        var pos =myCollider.ClosestPoint(attackPosition);
        var obj = Instantiate(hitParticlePrefab,pos,Quaternion.identity);
        var par = obj.GetComponent<ParticleSystem>();
        StartCoroutine(WaitDestroy(par));
        particleObjectList.Add(obj);

        if (CurrentStatus.Hp <= 0)
        {
            Debug.Log("HPが0になった");
            OnDie();
        }
        else
        {
            Debug.Log( damage + "のダメージを受けた！残りHP" + CurrentStatus.Hp);
        }
    }
    void OnDie()
    {
        Debug.Log("死亡した。");
        StopAllCoroutines();
        if(particleObjectList.Count>0)
        {
            foreach(var obj in particleObjectList) Destroy(obj);
            particleObjectList.Clear();
        }
        GameOverEvent?.Invoke();
    }
    IEnumerator WaitDestroy(ParticleSystem particle)
    {
        yield return new WaitUntil(()=>particle.isPlaying == false);
        if(particleObjectList.Contains(particle.gameObject)==true)particleObjectList.Remove(particle.gameObject);
        Destroy(particle.gameObject);
    }
    public void Retry()
    {
        // // 現在のステータスの初期化
        // CurrentStatus.Hp = DefaultStatus.Hp;
        // CurrentStatus.Power = DefaultStatus.Power;
        // // 一回転を初期位置に戻す
        // this.transform.position = startPosition;
        // this.transform.rotation = startRotation;

        // // 攻撃処理の途中でやられた時用
        // isAttack = false;

        // hpBar.value = CurrentStatus.Hp;
    }

    public void OnHeal(int healPoint)
    {
        CurrentStatus.Hp += healPoint;
        Debug.Log( "HPが" + healPoint + "回復!!" );

        if(CurrentStatus.Hp > DefaultStatus.Hp) CurrentStatus.Hp = DefaultStatus.Hp;

        hpBar.value = CurrentStatus.Hp; 
    }
    
    public void OnDamage(int hindranceDamage)
    {
        CurrentStatus.Hp -= hindranceDamage;
        Debug.Log( "HPが" + hindranceDamage + "減少！！" );

        if(CurrentStatus.Hp > DefaultStatus.Hp) CurrentStatus.Hp = DefaultStatus.Hp;

        hpBar.value = CurrentStatus.Hp;

        if (CurrentStatus.Hp <= 0)
        {
            Debug.Log("HPが0になった");
            OnDie();
        }
    }

    public enum events
    {
        go,
        stop,
    }

    public enum events2
    {
        go,
        stop,
    }

    public void YesClick()
    {
        StartCoroutine(Events());
    }
    public void NoClick()
    {
        panel.SetActive(false);
    }
    public void tpClick()
    {
       SceneManager.LoadScene("test");
    }
    public void NotpClick()
    {
        isPanel3ManuallyHidden = true;
        events3 = events2.stop;
        panel3.SetActive(false);

    }

    IEnumerator Events()
    {
        bar.SetActive(true);
        Destroy(panel);
        panel2.SetActive(true);
        yield return new WaitForSeconds(3);
        Destroy(panel2);
    }
}

