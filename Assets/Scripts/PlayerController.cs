using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float JumpForce = 10;
    public float JumpForceX = 10;
    public float Velocity = 10;
    public bool jumpKey = true;
    public int AttackNro = 1;
    public bool AttackMode = false;
    public bool life = true;
    public int Resistence = 5;
    public bool HitMode = false;
    public bool HitFlag = false;
    public Vector3 PuntoControl;
    public Text VidasText;

    private SpriteRenderer _spriteRenderer; // null
    private Rigidbody2D _rb;
    private Animator _animator;
    private BoxCollider2D _collider2D;
    private BoxCollider2D[] colliders;

    private static readonly string ANIMATOR_STATE = "State";
    private static readonly int ANIMATION_IDLE = 0;
    private static readonly int ANIMATION_RUN = 1;
    private static readonly int ANIMATION_ROLL = 2;
    private static readonly int ANIMATION_JUMP = 3;
    private static readonly int ANIMATION_ATTACK1 = 4;
    private static readonly int ANIMATION_ATTACK2 = 5;
    private static readonly int ANIMATION_ATTACK3 = 6;
    private static readonly int ANIMATION_DEATH = 7;

    private static readonly float RIGHT = 1;
    private static readonly float LEFT = -1;
    public float AuxDirec = 0;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D= GetComponent<BoxCollider2D>(); 
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        
        //VidasText.text = "Este es un nuevo texto";
    }

    // Update is called once per frame
    void Update()
    {
        var velocidadActualY = _rb.velocity.y;
        _rb.velocity = new Vector2(0, velocidadActualY);


        if (!life)
        {
            Debug.Log("MUERTO...");
            hit(2);
            colliders[1].enabled = false;

        }
        else if (HitFlag)
        {
            //Debug.Log("GOLPEADO...");
            colliders[1].enabled = false;
            hit(6);
            //HitFlag = false;
        }
        else
        {
            //Debug.Log("Direccion: "+AuxDirec.ToString());
            if (!Input.GetKey(KeyCode.RightArrow) || (!Input.GetKey(KeyCode.LeftArrow)))
            {
                //colliders[1].enabled = true;
                ChangeAnimation(ANIMATION_IDLE);
            }
            if (Input.GetKeyDown(KeyCode.Z) && jumpKey && (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.LeftArrow))))
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                JumpForceX = 3;
                AttackNro = 1;
                Salta();

            }
            else if (Input.GetKeyDown(KeyCode.X) && jumpKey && Rolling() && (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.LeftArrow))))
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                JumpForceX = 8;
                Rodar();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }
            if (Input.GetKey(KeyCode.RightArrow) && jumpKey && Attacking())
            {
                Desplazarse(RIGHT);
                AuxDirec = RIGHT;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && jumpKey && Attacking())
            {
                Desplazarse(LEFT);
                AuxDirec = LEFT;
            }
            

            Direccion(AuxDirec, JumpForceX);
        }
        AttackPass();
        HitPass();

        Physics2D.IgnoreLayerCollision(8,9);
        //Debug.Log(HitFlag);
        ViewLife();
    }

    void AttackPass()
    {
        if (!Attacking())
        {
            AttackMode = true;
            colliders[2].enabled = true;
        }
        else
        {
            AttackMode = false;
            colliders[2].enabled = false;
        }
    }
    void HitPass()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && !life)
        {
            transform.position = PuntoControl;
            life = true;
        }
        else
        {
            //colliders[1].enabled = true;
            //HitFlag = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Piso"))
        {
            Debug.Log("PISO...");
            jumpKey = true;
            if (HitFlag)
            {
                HitFlag = false;
                
            }
            colliders[1].enabled = true;

        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (AttackMode == true)
            {
                Destroy(other.gameObject);
            }
            else
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                
                jumpKey = false;
                this._rb.AddForce(Vector2.up * JumpForce / 2, ForceMode2D.Impulse);

                checkLife();
            }
            
        }
        if (other.gameObject.CompareTag("Caida"))
        {
            life = false;
        }
        if (other.gameObject.CompareTag("Boss"))
        {
            Debug.Log("FIN...");
            SceneManager.LoadScene(sceneName: "Creditos");
        }
        if (other.gameObject.CompareTag("Save"))
        {
            PuntoControl = other.gameObject.transform.position;
            Resistence = 6;

        }
    }
    private void Attack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            AttackNro = 2;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            AttackNro = 3;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            AttackNro = 1;
        }

        switch (AttackNro)
        {
            case 1:
                ChangeAnimation(ANIMATION_ATTACK1);
                break;
            case 2:
                ChangeAnimation(ANIMATION_ATTACK2);
                break;
            case 3:
                ChangeAnimation(ANIMATION_ATTACK3);
                break;
        }

    }
    private void Rodar()
    {
        this._rb.AddForce(Vector2.up * JumpForce/2, ForceMode2D.Impulse);
        //_spriteRenderer.transform.localScale = new Vector3(_spriteRenderer.transform.localScale.x, _spriteRenderer.transform.localScale.y, _spriteRenderer.transform.localScale.z-1);
        colliders[1].enabled = false;
        ChangeAnimation(ANIMATION_ROLL);
        jumpKey = false;
    }
    private void Salta()
    {
        this._rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        ChangeAnimation(ANIMATION_JUMP);
        jumpKey = false;
    }
    private void Direccion(float position, float veloc)
    {
        if (!jumpKey)
        {
            _rb.velocity = new Vector2(veloc * position, _rb.velocity.y);
        }
    }

    private void Desplazarse(float position)
    {
        ChangeAnimation(ANIMATION_RUN);
        _spriteRenderer.transform.localScale = new Vector3(5 * position, 5, 5);
        
        _rb.velocity = new Vector2(Velocity * position, _rb.velocity.y);
    }
    public bool Rolling()
    {
        return !_animator.GetCurrentAnimatorStateInfo(0).IsName("Roll");
    }
    public bool Attacking()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return false;
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            return false;
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
            return false;
        else
            return true;
        //return !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }
    public void checkLife()
    {
        if (Resistence <= 0)
        {
            life = false;
        }
        else
        {
            Resistence -= 1;
            HitFlag = true;
        }
    }
    public void hit(float ForceX)
    {
        ChangeAnimation(ANIMATION_DEATH);
        //JumpForceX = 2;
        Direccion(AuxDirec * -1, ForceX);
    }
    public void CollisionBody()
    {
        if (!Rolling())
            _collider2D.enabled = false;
        else
            _collider2D.enabled = true;
    }
    private void ChangeAnimation(int animation)
    {
        _animator.SetInteger(ANIMATOR_STATE, animation);
    }
    public void ViewLife()
    {
        switch (Resistence)
        {
            case 0:
                VidasText.text = "Vidas: ";
                break;
            case 1:
                VidasText.text = "Vidas: £";
                break;
            case 2:
                VidasText.text = "Vidas: £ £";
                break;
            case 3:
                VidasText.text = "Vidas: £ £ £";
                break;
            case 4:
                VidasText.text = "Vidas: £ £ £ £";
                break;
            case 5:
                VidasText.text = "Vidas: £ £ £ £ £";
                break;
            case 6:
                VidasText.text = "Vidas: £ £ £ £ £ £";
                break;
        }
    }
}
