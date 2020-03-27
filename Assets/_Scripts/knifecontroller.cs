using System.Collections;
using System.Collections.Generic;
using Unity.Burst;  
using UnityEngine;

public class knifecontroller : MonoBehaviour
{
    public float speed;

    public SpriteRenderer myRenderer;
  
    public Collider2D myCollider;

    public Rigidbody2D myRidgidbody;


    [HideInInspector]
    public bool shot;

    private Vector3 lastposition;

    private Vector3 intiatpos;


    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip crashSound;

    private AudioSource mAS;

    // Start is called before the first frame update
    void Start()
    {
        intiatpos = transform.position;

       // myCollider = GetComponent<Collider2D>();
        myRidgidbody = GetComponent<Rigidbody2D>();
        mAS = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
    
                  

        if (shot)
        {
            lastposition = transform.position;

            transform.position += Vector3.up * speed * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Linecast(lastposition, transform.position);

            if (hit.collider != null)
            {

                shot = false;
                if (hit.transform.tag == "knife")
                {
                   
                    // lose
                    LevelManger.instance.WrongHit();

                    myRidgidbody.bodyType = RigidbodyType2D.Dynamic;
                    myRidgidbody.AddTorque(10, ForceMode2D.Impulse);

                    mAS.clip = crashSound;
                    mAS.Play();

                }
                
                else
                {

                    transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                    transform.parent = hit.transform;
                    myCollider.enabled = true;
                    LevelManger.instance.Successfulhit(myRidgidbody);

                    mAS.clip = hitSound;
                    mAS.Play();

                }
              

            }

        }

    }

    public void Shoot()
    {
        shot = true;
        mAS.clip = shootSound;
        mAS.Play(); 
    }


    public void ShowAnimation()
    {
        StartCoroutine("ShowKnife");

    }
    IEnumerator ShowKnife()
    {

        yield return new WaitForEndOfFrame(); 
        float starttime = Time.time;
        float duration = 0.3f;
        // distance that i want knife come from it هتيجى من تحت قد ايه 
        Vector3 downpos = intiatpos - new Vector3(0, 0.5f, 0);

        //time that should animation for knife be played
        while (Time.time < starttime + duration)
        {
            float t = (Time.time - starttime) / duration;
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b,Mathf.Lerp(0,1,t));
            transform.position = Vector3.Lerp(downpos,intiatpos,t);

            yield return null;
        }
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1);
        transform.position = intiatpos;
    }


}
