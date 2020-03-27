using System.Collections;
using System.Collections.Generic;
using Unity.Burst;  
using UnityEngine;

public class targetcontroller : MonoBehaviour

{

     [SerializeField]
    private SpriteRenderer flasher;

    [SerializeField]
    private GameObject destroyedTarget;

    [SerializeField]
    private List<Rigidbody2D> destroyedTargetpices;

    private Vector3 targetintiatpos;

    private float roundRotationSpeed;
    private float roundStartTime;
    private float roundDuration;

    // Start is called before the first frame update
    void Start()
    {
        targetintiatpos = transform.position;
        NewRotationRound();
     
    }


    // Update is called once per frame
    void Update()
    {
        // this return number between 0 and 1 
        float t = (Time.time - roundStartTime) / roundDuration;
        // made this to makesure the number near to 1 and going to 0 
        t = 1 - t;
        float curRotationSpeed = roundRotationSpeed * t;
        transform.Rotate(new Vector3(0, 0, curRotationSpeed)* Time.deltaTime);
        if(t < 0.05f)
        {
            NewRotationRound();
        }
        
    }

    void NewRotationRound()
    {

        roundStartTime = Time.time;
        float roundPower = Random.Range(0, 1f);
        roundRotationSpeed = -100 - 100 * roundPower;
        roundDuration = 5 + 5 * roundPower;


    }


    public void GotHit()
    {
        StopCoroutine("Pushing");
        StopCoroutine("Flashing");

        StartCoroutine("Pushing");
        StartCoroutine("Flashing");
    }

    IEnumerator Pushing()
    {
        float starttime = Time.time;
        float duration = 0.025f;
        Vector3 uppostition = targetintiatpos + new Vector3(0, 0.1f, 0);
        while(Time.time<starttime+duration)
        {
            float t = (Time.time-starttime) / duration;

            transform.position = Vector3.Lerp(targetintiatpos, uppostition, t);
            yield return null;
            
        }

       starttime = Time.time;
        duration = 0.2f;

        while(Time.time<starttime+duration)
        {

            float t = (Time.time - starttime) / duration;
            // easy to return target slowly 
            t = 1 - Mathf.Abs(Mathf.Pow(t - 1, 2));


            transform.position = Vector3.Lerp(uppostition, targetintiatpos, t);
            yield return null;
        }
        transform.position = targetintiatpos;

    }

    IEnumerator Flashing()
    {
        float starttime = Time.time;
        float duration = 0.025f;
        Vector3 uppostition = targetintiatpos + new Vector3(0, 0.1f, 0);
        while (Time.time < starttime + duration)
        {
            float t = (Time.time - starttime) / duration;

            flasher.color = new Color(flasher.color.r, flasher.color.g, flasher.color.b, Mathf.Lerp(0,0.5f, t));
            yield return null;

        }

        starttime = Time.time;
        duration = 0.2f;

        while (Time.time < starttime + duration)
        {

            float t = (Time.time - starttime) / duration;
            // easy to return target slowly 
            t = 1 - Mathf.Abs(Mathf.Pow(t - 1, 2));

            flasher.color = new Color(flasher.color.r, flasher.color.g, flasher.color.b, Mathf.Lerp(0.5f, 0, t));

            yield return null;
        }
        flasher.color = new Color(flasher.color.r, flasher.color.g, flasher.color.b, 0);

        transform.position = targetintiatpos;


    }

     public void DestroyMe()
    {

        destroyedTarget.transform.parent = null;
        destroyedTarget.SetActive(true);

        
        for(int i=0;i<destroyedTargetpices.Count;i++)
        {
            Vector3 forcedirection = (destroyedTargetpices[i].transform.position - transform.position).normalized *4;

            // force to y axis always positve if it less than 0 mutiple it at -1 else keep it
            forcedirection.y = forcedirection.y < 0 ? forcedirection.y * -1 : forcedirection.y ;

            destroyedTargetpices[i].AddForceAtPosition(forcedirection, transform.position,ForceMode2D.Impulse);
            destroyedTargetpices[i].AddTorque(4, ForceMode2D.Impulse);

            Destroy(destroyedTargetpices[i].gameObject, 3);

        }

        Destroy(gameObject);

    }

}
