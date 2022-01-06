using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigHurtControl : MonoBehaviour
{
    [SerializeField]private int maxSpeed =8;
    [SerializeField]private int minSpeed=4;
    private SpriteRenderer renderer;
    [SerializeField] private Sprite sprite;//受伤状态
    [SerializeField] private GameObject Number;//分数
    [SerializeField]private int score;
    private int hp=100;

    public bool isPig;//是不是猪
    
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (hp<=0)
        {
            PigDie();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > maxSpeed)
        {
            hp -= 100;
        }
        else if (collision.relativeVelocity.magnitude > minSpeed && collision.relativeVelocity.magnitude < maxSpeed)
        {
            hp -= 50;
            renderer.sprite = sprite;
        }
    }

    void PigDie()
    {
        Destroy(gameObject);
        if (isPig)
        {
            GameManager.instance.pigs.Remove(this);
        }
        GameManager.instance.AddNum(score);
        GameObject boom = Instantiate(Resources.Load<GameObject>("Boom"));
        boom.transform.position = transform.position;
        Destroy(boom,0.25f);
        GameObject num = Instantiate(Number);
        num.transform.position = transform.position + new Vector3(0, 2);
        Destroy(num,1.3f);
    }
}
