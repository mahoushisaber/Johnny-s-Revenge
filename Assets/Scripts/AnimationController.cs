using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject slash;
    public GameObject daggerThrow;
    public GameObject siphon;
    public GameObject potionHeal;
    public GameObject powerStrike;
    public GameObject saberAttack;
    public GameObject pierce;

    void Update()
    {
        ProcessInputs();
    }

    void ProcessInputs()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject spawnedSlash = Instantiate(slash, new Vector3(51, -16, 65), slash.transform.rotation);
            spawnedSlash.GetComponent<Animator>().Play("slash");
            Destroy(spawnedSlash, 1.0f);
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject daggers = Instantiate(daggerThrow, this.transform.position, daggerThrow.transform.rotation);
            daggers.GetComponent<Animator>().Play("singleDaggerThrow");
            Destroy(daggers, 1.5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject potion = Instantiate(potionHeal, new Vector3(50, 0, 30), potionHeal.transform.rotation);
            potion.GetComponent<Animator>().Play("heal");
            Destroy(potion, 2.25f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject hook = Instantiate(siphon, new Vector3(50, 15, 30), siphon.transform.rotation);
            hook.GetComponent<Animator>().Play("siphon");
            Destroy(hook, 1.5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject strike = Instantiate(powerStrike, new Vector3(50, 15, 30), powerStrike.transform.rotation);
            strike.GetComponent<Animator>().Play("powerStrike");
            Destroy(strike, 3.0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
 
            GameObject saber = Instantiate(saberAttack, new Vector3(51, -16, 65), saberAttack.transform.rotation);
            saber.GetComponent<Animator>().Play("saberAttack");
            Destroy(saber, 1.5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        { 
            GameObject spawnedPierce = Instantiate(pierce, new Vector3(51, 10, 85), pierce.transform.rotation);
            spawnedPierce.GetComponent<Animator>().Play("pierce");
            Destroy(spawnedPierce, 2.0f);
        }

    }
}
