using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float smoothing = 1;
    public float restTime = 1;

    public AudioClip chop1;
    public AudioClip chop2;


    private float restTimer = 0;

    [HideInInspector]public  Vector2 targetPos = new Vector2(1, 1);
    private Rigidbody2D rigidBody;
    private BoxCollider2D colliders;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        colliders = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.MovePosition(Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime));
        if (GameManager.Instance.food <= 0 || GameManager.Instance.isEnd == true)
        {
            return;
        }

        restTimer += Time.deltaTime;
        if(restTimer<restTime)
            return;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if(h > 0)
        {
            v = 0;
        }
        if(h != 0 ||v != 0)
        {
            GameManager.Instance.subFood(1);
            //检测
            colliders.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos+new Vector2(h,v));
            colliders.enabled = true;
            if(hit.transform == null)
            {
                targetPos += new Vector2(h, v);
            }
            else
            {
                switch(hit.transform.tag)
                {
                    case "outWall":
                        break;
                    case "Wall":
                        animator.SetTrigger("Attack");
                        AudioManager.Instance.RandomPlay(chop1,chop2);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        GameManager.Instance.addFood(10);
                        targetPos += new Vector2(h, v);
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Soda":
                        GameManager.Instance.addFood(20);
                        targetPos += new Vector2(h, v);
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Enemy":
                        break;
                }

            }
            GameManager.Instance.OnPlayerMove();
            restTimer = 0;
        }
        
        
    }

    public void TakeDamage(int lossFood)
    {
        GameManager.Instance.subFood(lossFood);
        animator.SetTrigger("Damage");
    }
}
