using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float smoothing = 3;
    public int lossFood = 10;

    private Transform player;
    private Vector2 targetPosition;
    private Rigidbody2D rigidBody;

    private BoxCollider2D collidering;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        GameManager.Instance.enemyList.Add(this);
        collidering = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        rigidBody.MovePosition( Vector2.Lerp(transform.position, targetPosition, smoothing* Time.deltaTime));
    }
    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if(offset.magnitude < 1.1f)
        {
            //攻击
            animator.SetTrigger("Attack");
            player.SendMessage("TakeDamage",lossFood);
        }
        else
        {
            float x = 0, y = 0;
            if(Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                //按照Y轴移动
                if (offset.y < 0) y = -1;
                else y = 1;
            }
            else
            {
                //按照X轴移动
                if (offset.x < 0) x= -1;
                else x = 1;
            }
            //设置目标位置之前，先做检测
            collidering.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y));
            collidering.enabled = true;
            if(hit.transform == null)
            {
                targetPosition += new Vector2(x, y);
            }
            else
            {
                if(hit.collider.tag == "Food" || hit.collider.tag == "Soda")
                {
                    targetPosition += new Vector2(x, y);
                }
            }

        }

    }

}
