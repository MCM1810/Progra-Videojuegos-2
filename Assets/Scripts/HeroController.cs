
using System;
using UnityEngine;
using UnityEngine.UI;

/* GRASHITAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA 
 * Hola jsjsjsjs xd
 * Bueno, he aqui mi avance, ya esta completo la pregunta 1 y 2, ahora con shifuu se 
 * encargara de hacer la ultima.
 * Pero antes de idealizar su plan, les comentare unas cosas para que los tomen en cuenta:
 * 
 * 1. Por cada plataforma que vayan a hacer, les pediria que creen un objeto RigBody 
 * llamado "Zona Muerte"(Se daran cuenta cuando vean el unity), 
 * esto ayudara a que el heroe muera cada vez que se golpee dentro de la pared al usar dash
 * 
 * 2.Puede cambiar la velocidad del dash pero no el tiempo maximo (es lo mejor que pude para
 * que la distancia sea fija). En la pestana Dash encontraran los valores publicos
 * 
 * 3.El heroe solo muere al chocar con la pared. Intente que el heroe se muera por el precipicio
 * o al acabar su vida
 * 
 * 4. Hagan un efecto chevere de la muerte, ya esta la funcion "Respawn"
 * 
 * 5. Hay conjunto de sprites del jefe llamado "Orange slime", se ve chevere xd. Pero si
 * no les gusta lo pueden cambiar
 * 
 * 6. La barra amarilla indica el poder acumulado del heroe
 * 
 * 7.La barra se llena completamente al destruiri a un solo enemigo, si quiere que se llene 
 * mas lento, deben de cambiar el valor publico "Aumento Poder" (aparece como 0.25)
 * 
 * 8.La barra de vida no esta implementada
 * 
 * 9. Enfoquense en un nivel tipo boss
 * 
 * 10.En cuanto los efectos de sonido, escuche una musica que puede ser chevere en 
 * el juego: https://www.youtube.com/watch?v=6gtAZIZQCuw&ab_channel=BrunuhVille esta piola :D
 * 
 * 11.Si en caso se les queda sin ideas del boss, aqui les dejo una recomendacion mia :D : 
 * https://www.youtube.com/watch?v=ZTMGZHztl-g&ab_channel=BossBattleChannel
 * 
 * Bueno, eso es todo de mi parte, me avisan cualquier cosa. Les deseo suerte uwu
 * 
 */
public class HeroController : MonoBehaviour
{
    private bool mIsCharged = false;
    public int NumSaltos;

    [SerializeField] private Transform PuntoRespawn;
    [SerializeField] private Transform Player;

    [Header("Movement")]
    public float moveSpeed;
    public float accel;
    public float deccel;
    public float speedExp;

    [Header("Jump")]
    public float raycastDistance;
    public float jumpForce;
    public float fallMultiplier;

    [Header("Fire")]
    public GameObject fireball; //prefab
    private Transform mFireballPoint;

    [Header("Dash")]
    public bool Dash;
    private float Dash_T = 0;
    public float Tiempo_Dash_Max = 0.35f;
    public float Speed_Dash;

    [Header("Vida")]
    public int NumVida;
    public Slider HeathBar;
 

    [Header("Poder")]
    public int NumPoder;
    public Slider PoweBar;
    public float AumentoPoder;
    

    private Rigidbody2D mRigidBody;
    private float mMovement;
    private Animator mAnimator;
    private SpriteRenderer mSpriteRenderer;
    

    private void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mFireballPoint = transform.Find("FireballPoint");
       

        
    }

    private void Update()
    {
        mMovement = Input.GetAxis("Horizontal");
        mAnimator.SetInteger("Move", mMovement == 0f ? 0 : 1);

        if (mMovement < 0f && !Dash)
        {
            //mSpriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(
                0f,
                180f,
                0f
            );
        } else if (mMovement > 0 && !Dash)
        {
            //mSpriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                0f
            );
        }

        bool isOnAir = IsOnAir();
      

        if (Input.GetButtonDown("Jump") && (this.NumSaltos < 1 || !isOnAir))
        {
            Jump(isOnAir);

        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Dash_Skill(mIsCharged);
        }
        else
        {
            Dash = false;
            mAnimator.SetBool("Dash", Dash);
            Dash_T = 0;
        }
    }


    private void FixedUpdate()
    {
        Move();

        if (mRigidBody.velocity.y < 0 && !Dash)
        {
            // Esta cayendo
            mRigidBody.velocity += (fallMultiplier - 1) * 
                Time.fixedDeltaTime * Physics2D.gravity;
        }
    }

    private void Move()
    {
        float targetSpeed = mMovement * moveSpeed;
        float speedDif = targetSpeed - mRigidBody.velocity.x;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? accel : deccel;
        float movement = Mathf.Pow(
            accelRate * Mathf.Abs(speedDif),
            speedExp
        ) * Mathf.Sign(speedDif);

        mRigidBody.AddForce(movement * Vector2.right);
    }

    private void Jump(bool isOnAir)
    {
        if (isOnAir)
        {
            mAnimator.SetBool("NextJump", true);
        }
        mRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        NumSaltos +=1;


    }

    public bool IsOnAir()
    {
        Transform rayCastOrigin = transform.Find("RaycastPoint");
        RaycastHit2D hit = Physics2D.Raycast(
            rayCastOrigin.position,
            Vector2.down,
            raycastDistance
        );
        mAnimator.SetBool("IsJumping", !hit);

        if (hit == true)
        {
            NumSaltos = 0;
            mAnimator.SetBool("NextJump", false);
            jumpForce = 7;

        }
        else
        {
            if (Dash)
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            else
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

        /*Color rayColor;
        if (hit)
        {
            rayColor = Color.red;
        }else
        {
            rayColor = Color.blue;
        }
        Debug.DrawRay(rayCastOrigin.position, Vector2.down * raycastDistance, rayColor);*/

        return !hit;
       
        
        //return hit == null ? true : false;
        
    }
    private void Fire()
    {
        mFireballPoint.GetComponent<ParticleSystem>().Play(); // ejecutamos PS
        GameObject obj = Instantiate(fireball, mFireballPoint);
        obj.transform.parent = null;
        
    }

    public Vector3 GetDirection()
    {
        return new Vector3(
            transform.rotation.y == 0f ? 1f : -1f,
            0f,
            0f
        );
    }

    public void Dash_Skill(bool IsCharged)
    {
    if(IsCharged)
        {
            Dash_T += 1 * Time.deltaTime;
            Debug.Log(Dash_T);
            if (Dash_T < Tiempo_Dash_Max)
            {
                Dash = true;
                mAnimator.SetBool("Dash", Dash);
                transform.Translate(Vector3.right * Speed_Dash * Time.deltaTime);
                
                
            }
            
            else
            {
                Dash = false;
                mAnimator.SetBool("Dash", Dash);
                mIsCharged = false;
            }
            this.PoweBar.value = 0;
           
        }




    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("ZonaMuerte"))
        {
            Respawn();
        }
        
    }

    public void Respawn()
    {
        Debug.Log("Paso por respawn");
        Player.transform.position = PuntoRespawn.transform.position;
    }

    public void Recarga()
    {
        
        PoweBar.value += AumentoPoder;
        Debug.Log(this.PoweBar.value);
        if(PoweBar.value == 1)
        {
            mIsCharged = true;
        }
    }
}
