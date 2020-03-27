using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelManger : MonoBehaviour
{
    public static LevelManger instance;         

    public int knifescount;
    [SerializeField]
    private GameObject knifeGO;
    [SerializeField]
    private Transform knifespwanpoint;

    [SerializeField]
    private List<Image> knifeicons;

    [SerializeField]
    private targetcontroller target;

    private int currentknifecount;
    private int hits;

    private List<Rigidbody2D> curhitknifes = new List<Rigidbody2D>();

    private knifecontroller currknife;

    private bool canPlay = true;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        GenerateLevel();
    }

          // generate knifes on target randomly 
    public void GenerateLevel()
    {
        knifescount = Random.Range(5, 16);
        //    already sticking knife at target 3

        for (int i = 0; i < knifeicons.Count; i++)
        {
            if(i >=knifescount)
            {
                knifeicons[i].color = new Color(knifeicons[i].color.r, knifeicons[i].color.g, knifeicons[i].color.b, 0);
            }

        }
        int sticknigKnife = Random.Range(0, 4);

        // maxangle that maximam angle must knife spirite before it  كل السكاكين هتتوزع قبل ماتوصل للرقم ده 
        float maxAngle = 360 / (float)sticknigKnife;
        float lastAngle = 0;
        for(int i=0;i<sticknigKnife;i++)
        {
            // الدرجه بتاعه السكينه الجديده هتكون بعد القديمه ب 20 درجه ومش هتزيد عن ال max
            float angle = lastAngle + Random.Range(20, maxAngle) * Mathf.Deg2Rad;
            lastAngle = angle;
            // postion of stcking knife i use sign and cos to draw circle and multible it by 3 this raduis of target 
            Vector3 pos = target.transform.position + new Vector3(Mathf.Sign(angle), Mathf.Cos(angle), 0) * 1.25f;
            GameObject knife = Instantiate(knifeGO, pos, Quaternion.identity);
            //   to set knife refer to center of target 
            knife.transform.up = target.transform.position - knife.transform.position;
            knife.transform.parent = target.transform;
            knifecontroller knifeBehavior = knife.GetComponent<knifecontroller>();
            knifeBehavior.myCollider.enabled = true;
            curhitknifes.Add(knifeBehavior.myRidgidbody);

        }

        currentknifecount = knifescount;
        SpwanKnife();


    }

    // Update is called once per frame
    void Update()
    {

        if (!canPlay)
        {
            return ;
        }
        if (Input.GetMouseButtonDown(0)&& currknife !=null && currentknifecount>-1)
        {
            ShootAKnife();
        }
        
    }

    void ShootAKnife()
    {
        currknife.Shoot();
        currentknifecount --;

        knifeicons[currentknifecount].color = new Color(0, 0, 0, 0.5f);

        if (currentknifecount > 0)
        {
            //spwanknife
            SpwanKnife();

        }
        else currknife = null;

    }
    void SpwanKnife()
    {
        GameObject knife = Instantiate(knifeGO, knifespwanpoint.position, Quaternion.identity);
        currknife = knife.GetComponent<knifecontroller>();
        currknife.ShowAnimation();
    }

    public void Successfulhit(Rigidbody2D knife)
    {
        target.GotHit();
        hits++;

        curhitknifes.Add(knife);
        GameManger.instance.AddScore();
        if (hits == knifescount)
        {
            Win();
        }
    }
    public void WrongHit()
    {
        Lose();
    }

    void Win()

    {
       

        for(int i=0;i<curhitknifes.Count;i++)
        {

            Rigidbody2D cknife = curhitknifes[i];
            cknife.transform.parent = null;
            cknife.bodyType = RigidbodyType2D.Dynamic;
            Vector3 forcedirection = (cknife.transform.position - target.transform.position).normalized * 4;
            // to make last knife up more
            if (i == curhitknifes.Count - 1)
            {
                forcedirection.y = 8;
            }
            cknife.AddForceAtPosition(forcedirection, target.transform.position, ForceMode2D.Impulse);
            cknife.AddTorque(4, ForceMode2D.Impulse);
            Destroy(cknife, 3);
        }

        target.DestroyMe();
        GameManger.instance.ReloadScene();

    }
     void Lose()
    {
        canPlay = false;
        GameManger.instance.ReloadScene();
        GameManger.instance.ResetScore();



    }

}
