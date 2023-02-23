using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
      [Header("PlayerCondition")]
    [SerializeField] int maxHp = 100; //new
    public int Hp; //re
    [SerializeField] int maxOx = 100; // new
    public int Ox; //new
    [SerializeField] float timer = 0; //new

    [Header("AboutControll")]
    [SerializeField] float jumpPower;
    [SerializeField] bool isGrounded;
    [Header("SpritesForCondition")]
    [SerializeField] GameObject run;
    [SerializeField] GameObject slide;
    [SerializeField] GameObject jump;
 
    [SerializeField] public bool isDie;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip a_Jump;
    [SerializeField] AudioClip a_Damaged;
    [SerializeField] AudioClip a_Food;
    [SerializeField] AudioClip a_Ox;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
       
    }
    void Start()
    {

        // a_Jump = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/Jump.wav",typeof(AudioClip));
        // a_Damaged = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/Damaged.wav", typeof(AudioClip));
        // a_Food = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/Food.wav", typeof(AudioClip));
        // a_Ox = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/Ox.wav", typeof(AudioClip));
        a_Jump = Resources.Load<AudioClip>("Sound/Jump");
        a_Damaged = Resources.Load<AudioClip>($"Sound/Damaged");
        a_Food = Resources.Load<AudioClip>($"Sound/Food");
        a_Ox = Resources.Load<AudioClip>($"Sound/Ox");

        Hp = maxHp; //new
        Ox = maxOx; //new
        //InvokeRepeating("TimerDamaged", 1f,1f);
        InvokeRepeating("TimerOxDamaged", 1f, 1f);//new
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP : " + Hp + " / Ox : " + Ox);//new
        if (Ox == 0)//new
        {
            //emOx = true;
            TimerDamaged(3);
        }
        Controll();

        if(this.Hp <=0)
        {
            this.isDie = true;
        }
        if(this.isDie == true)
        {
            Hp = 0;
        }
        //CheckGround();    
    }
    void Controll()
    {
        if (Time.deltaTime == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            run.SetActive(false);
        }
        //바닥에 닿아있을때
        else if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            slide.SetActive(true);
            run.SetActive(false);
            jump.SetActive(false);
            this.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.2f);
            this.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.7f);
        }
        else if (isGrounded)
        {
            slide.SetActive(false);
            run.SetActive(true);
            jump.SetActive(false);
            this.GetComponent<BoxCollider2D>().size = new Vector2(1, 3.2f);
            this.GetComponent<BoxCollider2D>().offset = new Vector2(0, 1.7f);
        }
        else if (!isGrounded)
        {
            slide.SetActive(false);
            jump.SetActive(true);
            run.SetActive(false);
        }

        if(this.isDie == true)
        {
            jump.SetActive(false);
            run.SetActive(false);
        }

    }
   
    void Jump()
    {
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        PlaySound(a_Jump);
    }
    void TimerDamaged(int timeDamage)//re
    {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            Damaged(timeDamage);
            timer = 0;
        }
       
    }
    void TimerOxDamaged() //new
    {
        Ox -= 5;
        Ox = Mathf.Clamp(Ox, 0, maxOx);
    }
    void OxHealing(int heal) //new
    {
        Ox += heal;
       
        Ox = Mathf.Clamp(Ox, 0, maxOx);
        PlaySound(a_Ox);
    }

    public void Damaged(int damage)
    {
        this.Hp -= damage;
        Hp = Mathf.Clamp(Hp, 0, maxHp); //new

    }

    void Healing(int heal) //new
    {
        this.Hp += heal;
        Hp = Mathf.Clamp(Hp, 0, maxHp); //new
        PlaySound(a_Food);

    }

    void PlaySound(AudioClip clip)
    {
        this.audioSource.PlayOneShot(clip);
    }
    //void CheckGround()
    //{
       
    //    if(Physics2D.Raycast(this.transform.position, Vector2.down,2f,layerMask))
    //    {
    //        this.isGrounded = true;            
    //    }
    //    else
    //    {
    //        this.isGrounded = false;
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            this.isGrounded = true;
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            this.Damaged(collision.gameObject.GetComponent<Obstacle>().damage);
            PlaySound(a_Damaged);

        }
        if (collision.gameObject.tag == "Food") //new
        {
            this.Healing(collision.gameObject.GetComponent<Obstacle>().heal);

        }
        if (collision.gameObject.tag == "Ox") // new
        {
            this.OxHealing(collision.gameObject.GetComponent<Obstacle>().oxHeal);

        }
        collision.gameObject.SetActive(false);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            this.isGrounded = false;
        }
       
    }

}
