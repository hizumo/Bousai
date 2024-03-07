using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PlayerController_Elv : MonoBehaviour
{


    public AudioClip sound1;
    AudioSource audioSource;

    public static PlayerController_Elv instance;




    [SerializeField] GameObject attackHit = null;
    [SerializeField] float jumpPower = 20f;
    [SerializeField] ColliderCallReceiver footColliderCall = null;
    [SerializeField] GameObject touchMarker = null;
    [SerializeField] PlayerCameraController cameraController = null;
    // �U���q�b�g�I�u�W�F�N�g��ColliderCall
    [SerializeField] ColliderCallReceiver attackHitCall = null;
    // ���g�̃R���C�_�[
    [SerializeField] Collider myCollider = null;
    // �U�����󂯂����̃p�[�t�F�N�g�v���n�u
    [SerializeField] GameObject hitParticlePrefab = null;
    // �p�[�e�B�N���I�u�W�F�N�g�ۊǃ��X�g
    List<GameObject> particleObjectList = new List<GameObject>();
    [SerializeField] Slider hpBar = null;
    [SerializeField] ColliderCallReceiver aroundColliderCall = null;
    Animator animator = null;
    Rigidbody rigid = null;
    bool isAttack = false;
    bool isGround = false;
    [System.Serializable]


    public class Status
    {
        // �̗�
        public int Hp = 10;
        // �U����
        public int Power = 1;
    }
    // ��{�X�e�[�^�X
    [SerializeField] Status DefaultStatus = new Status();
    // ���݂̃X�e�[�^�X
    public Status CurrentStatus = new Status();


    // PC�L�[����������
    float horizontalKeyInput = 0;
    // PC�L�[�c��������
    float verticalKeyInput = 0;
    // �Q�[���I�[�o�[���C�x���g
    public UnityEvent GameOverEvent = new UnityEvent();
    // �J�n�ʒu
    Vector3 startPosition = new Vector3();
    // �J�n�p�x
    Quaternion startRotation = new Quaternion();

    int hindranceDamage = 2;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

        audioSource = GetComponent<AudioSource>();


        animator = GetComponent<Animator>();
        attackHit.SetActive(false);
        rigid = GetComponent<Rigidbody>();
        footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
        footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
        // �U������p�R���C�_�[�C�x���g�o�^
        attackHitCall.TriggerEnterEvent.AddListener(OnAttackHitTriggerEnter);
        // ��Q���_���[�W�R���C�_�[�C�x���g
        // aroundColliderCall.TriggerStayEvent.AddListener( OnHindranceTriggerStay );
        // ���݂̃X�e�[�^�X�̏�����
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        // �J�n���̈��]��ۊ�
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;

        // �X���C�_�[�̏����ݒ�
        hpBar.maxValue = DefaultStatus.Hp;
        hpBar.value = CurrentStatus.Hp;
    }

    bool isTouch = false;
    // �������^�b�`�X�^�[�g�ʒu
    Vector2 LeftStartTouch = new Vector2();
    // �������^�b�`����
    Vector2 LeftTouchInput = new Vector2();

    // Update is called once per frame
    void Update()
    {



        // �J�������v���C���[�Ɍ�����
        cameraController.UpdateCameraLook(this.transform);
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // �X�}�z�^�b�`����
            // �^�b�`���Ă���w�̐���0��葽��
            if (Input.touchCount > 0)
            {
                isTouch = true;
                // �^�b�`�������ׂĎ擾
                Touch[] touches = Input.touches;
                // �S���̃^�b�`���J��Ԃ��Ĕ���
                foreach (var touch in touches)
                {
                    bool isLeftTouch = false;
                    bool isRightTouch = false;
                    // �^�b�`�ʒu��X���������X�N���[���̍���
                    if (touch.position.x > 0 && touch.position.x < Screen.width / 2)
                    {
                        isLeftTouch = true;
                    }
                    // �^�b�`�ʒu��X���������X�N���[���̉E��
                    else if (touch.position.x > Screen.width / 2 && touch.position.x < Screen.width)
                    {
                        isRightTouch = true; ;
                    }
                    // ���^�b�`
                    if (isLeftTouch == true)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            Debug.Log("�^�b�`�J�n");
                            // �J�n�ʒu��ۊ�
                            LeftStartTouch = touch.position;
                            // �J�n���Ƀ}�[�J�[��\��
                            touchMarker.SetActive(true);
                            Vector3 touchPosition = touch.position;
                            touchPosition.z = 1f;
                            Vector3 markerPosition = Camera.main.ScreenToViewportPoint(touchPosition);
                            touchMarker.transform.position = markerPosition;

                        }
                        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                        {
                            Debug.Log("�^�b�`��");
                            // ���ݒn��ۊ�
                            Vector2 position = touch.position;
                            // �ړ��p�̕�����ۊ�
                            LeftTouchInput = position - LeftStartTouch;
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            Debug.Log("�^�b�`�I��");
                            LeftTouchInput = Vector2.zero;
                            // �}�[�J�[���\��
                            touchMarker.gameObject.SetActive(false);
                        }
                    }
                    // �E�^�b�`
                    if (isRightTouch == true)
                    {
                        // �E�������^�b�`�������̏���
                        cameraController.UpdateRightTouch(touch);
                    }
                }
            }
            else
            {
                isTouch = false;
            }
        }
        else
        {// PC�L�[���͎擾
            horizontalKeyInput = Input.GetAxis("Horizontal");
            verticalKeyInput = Input.GetAxis("Vertical");
        }
        // �v���C���[�̌����𒲐�
        bool isKeyInput = (horizontalKeyInput != 0 || verticalKeyInput != 0 || LeftTouchInput != Vector2.zero);
        if (isKeyInput == true && isAttack == false)
        {
            bool currentIsRun = animator.GetBool("isRun");
            if (currentIsRun == false) animator.SetBool("isRun", true);
            Vector3 dir = rigid.velocity.normalized;
            dir.y = 0;
            this.transform.forward = dir;
            Debug.Log("Run");
        }
        else
        {
            bool currentIsRun = animator.GetBool("isRun");
            if (currentIsRun == true) animator.SetBool("isRun", false);
        }



    }

    void FixedUpdate()
    {
        if (isAttack == false)
        {
            Vector3 input = new Vector3();
            Vector3 move = new Vector3();
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                input = new Vector3(LeftTouchInput.x, 0, LeftTouchInput.y);
                move = input.normalized * 2f;
            }
            else
            {
                input = new Vector3(horizontalKeyInput, 0, verticalKeyInput);
                move = input.normalized * 2f;
            }

            Vector3 cameraMove = Camera.main.gameObject.transform.rotation * move;
            cameraMove.y = 0;
            Vector3 currentRigidVelocity = rigid.velocity;
            currentRigidVelocity.y = 0;

            rigid.AddForce(cameraMove - rigid.velocity, ForceMode.VelocityChange);
        }
        cameraController.FixedUpdateCameraPosition(this.transform);
    }

    public void OnAttackButtonClicked()
    {
        if (isAttack == false)
        {
            animator.SetTrigger("isAttack");
            isAttack = true;
        }
    }
    public void OnJumpButtonClicked()
    {
        if (isGround == true)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            Debug.Log("�W�����v�{�^����������");
        }
    }
    void OnFootTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (isGround == false) isGround = true;
            if (animator.GetBool("isGround") == false) animator.SetBool("isGround", true);
        }
    }
    void OnFootTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGround = false;
            animator.SetBool("isGround", false);
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
        if (col.gameObject.tag == "Danger")
        {
            var enemy = col.gameObject.GetComponent<Danger>();
            enemy?.OnAttackHit(CurrentStatus.Power, this.transform.position);
            attackHit.SetActive(false);
        }
    }
    public void OnEnemyAttackHit(int damage, Vector3 attackPosition)
    {
        CurrentStatus.Hp -= damage;
        var pos = myCollider.ClosestPoint(attackPosition);
        var obj = Instantiate(hitParticlePrefab, pos, Quaternion.identity);
        var par = obj.GetComponent<ParticleSystem>();
        StartCoroutine(WaitDestroy(par));
        particleObjectList.Add(obj);

        if (CurrentStatus.Hp <= 0)
        {
            OnDie();
        }
        else
        {
            Debug.Log(damage + "�̃_���[�W���󂯂��I�c��HP" + CurrentStatus.Hp);
        }
    }
    void OnDie()
    {
        Debug.Log("���S�����B");
        StopAllCoroutines();
        if (particleObjectList.Count > 0)
        {
            foreach (var obj in particleObjectList) Destroy(obj);
            particleObjectList.Clear();
        }
        GameOverEvent?.Invoke();
    }
    IEnumerator WaitDestroy(ParticleSystem particle)
    {
        yield return new WaitUntil(() => particle.isPlaying == false);
        if (particleObjectList.Contains(particle.gameObject) == true) particleObjectList.Remove(particle.gameObject);
        Destroy(particle.gameObject);
    }
    public void Retry()
    {
        // // ���݂̃X�e�[�^�X�̏�����
        // CurrentStatus.Hp = DefaultStatus.Hp;
        // CurrentStatus.Power = DefaultStatus.Power;
        // // ���]�������ʒu�ɖ߂�
        // this.transform.position = startPosition;
        // this.transform.rotation = startRotation;

        // // �U�������̓r���ł��ꂽ���p
        // isAttack = false;

        // hpBar.value = CurrentStatus.Hp;
    }

    public void OnHeal(int healPoint)
    {
        CurrentStatus.Hp += healPoint;
        Debug.Log("HP��" + healPoint + "��!!");

        if (CurrentStatus.Hp > DefaultStatus.Hp) CurrentStatus.Hp = DefaultStatus.Hp;

        hpBar.value = CurrentStatus.Hp;
    }

    public void OnDamage(int hindranceDamage)
    {
        CurrentStatus.Hp -= hindranceDamage;
        Debug.Log("HP��" + hindranceDamage + "�����I�I");

        if (CurrentStatus.Hp > DefaultStatus.Hp) CurrentStatus.Hp = DefaultStatus.Hp;

        hpBar.value = CurrentStatus.Hp;
    }
}

