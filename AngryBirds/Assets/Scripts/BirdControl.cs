using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    public Transform slingshorPos;//弹弓点
    private float maxDis = 1.3f;//最大距离
    private bool birdMove;

    [HideInInspector] public Rigidbody2D rigidbody;
    [HideInInspector] public SpringJoint2D springJoint;

    public LineRenderer right;
    public LineRenderer left;
    public Transform rightPos;
    public Transform leftPos;
    private TestMyTrail testMyTrail;
    private bool canMove = true;
    private bool birdSkill;//鸟的技能
    public AudioClip select;//选择音乐
    public AudioClip fly;//发射音乐
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        springJoint = GetComponent<SpringJoint2D>();
        testMyTrail = GetComponent<TestMyTrail>();
    }
    private void OnMouseDown()
    {
        if (canMove)
        {
            birdMove = true;
            springJoint.enabled = true;
            rigidbody.isKinematic = true;
            AudioSource.PlayClipAtPoint(select,transform.position);
        }
    }

    private void OnMouseUp()
    {
        if (canMove)
        {
            AudioSource.PlayClipAtPoint(fly,transform.position);
            right.enabled = false;
            left.enabled = false;
            birdMove = false;
            rigidbody.isKinematic = false;
            Invoke("BirdFly", 0.1f);
            canMove = false;
        }
    }

    private void FixedUpdate()
    {
        Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position,
            new Vector3(Mathf.Clamp(transform.position.x,0,10),Camera.main.transform.position.y, Camera.main.transform.position.z),
            2*Time.deltaTime);
    }
    private void Update()
    {
        if (birdMove)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += new Vector3(0, 0, 10);
            if (Vector3.Distance(transform.position, slingshorPos.position) > maxDis)
            {
                Vector3 pos = (transform.position - slingshorPos.position).normalized;
                pos *= maxDis;//最大长度
                transform.position = pos + slingshorPos.position;
            }
            DrawLine();
        }

        if (birdSkill&&Input.GetMouseButtonDown(0))
        {
            UseSkill();
        }
    }
    /// <summary>
    /// 扩展技能 重写此方法
    /// </summary>
    public virtual void UseSkill()
    {
        birdSkill = false;
    }
    void BirdFly()
    {
        birdSkill = true;
        testMyTrail.StartTrails();
        springJoint.enabled = false;
        Invoke("NextBird", 3.5f);
    }
    /// <summary>
    /// 画弹弓线
    /// </summary>
    void DrawLine()
    {
        right.enabled = true;
        left.enabled = true;
        right.SetPosition(0, rightPos.position);
        right.SetPosition(1, transform.position);
        left.SetPosition(0, leftPos.position);
        left.SetPosition(1, transform.position);
    }

    void NextBird()
    {
        GameManager.instance.birds.Remove(this);
        Destroy(gameObject);
        GameObject boom = Instantiate(Resources.Load<GameObject>("Boom"), transform.position, Quaternion.identity);
        boom.transform.localScale = transform.localScale;
        Destroy(boom, 0.25f);
        GameManager.instance.WinCondition();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        birdSkill = false;
        testMyTrail.ClearTrails();
    }
}
