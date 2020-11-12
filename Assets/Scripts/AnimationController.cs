using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationController : MonoBehaviour
{
    public GameObject slash;
    public GameObject daggerThrow;
    public GameObject siphon;
    public GameObject potionHeal;
    public GameObject powerStrike;
    public GameObject powerStrikeNoMana;
    public GameObject saberAttack;
    public GameObject pierce;

    private PlayerController Player;

    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        ProcessInputs();
    }

    void ProcessInputs() // For testing purposes
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaySlash();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayDaggerThrow();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayHeal();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlaySiphon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayPowerStrike();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlaySaberAttack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlayPierce();
        }

    }

    public void PlaySlash()
    {
        GameObject spawnedSlash = Instantiate(slash, new Vector3(51, -16, 65), slash.transform.rotation);
        spawnedSlash.GetComponent<Animator>().Play("slash");
        Destroy(spawnedSlash, 1.0f);
    }

    public void PlayDaggerThrow()
    {
        GameObject daggers = Instantiate(daggerThrow, this.transform.position, daggerThrow.transform.rotation);
        daggers.GetComponent<Animator>().Play("singleDaggerThrow");
        Destroy(daggers, 1.5f);
    }

    public void PlayHeal()
    {
        GameObject potion = Instantiate(potionHeal, new Vector3(50, 0, 30), potionHeal.transform.rotation);
        potion.GetComponent<Animator>().Play("heal");
        Destroy(potion, 2.25f);
    }

    public void PlaySiphon()
    {
        GameObject hook = Instantiate(siphon, new Vector3(50, 15, 30), siphon.transform.rotation);
        hook.GetComponent<Animator>().Play("siphon");
        Destroy(hook, 1.5f);
    }

    public void PlayPowerStrike()
    {
        if (Player.Mana >= 50 && Player.Mana < 100)
        {
            GameObject strike = Instantiate(powerStrike, new Vector3(51, -16, 65), powerStrike.transform.rotation);
            strike.GetComponent<Animator>().Play("powerStrike");
            Destroy(strike, 2.5f);
        }
        else
        {
            GameObject strike = Instantiate(powerStrikeNoMana, new Vector3(51, -16, 65), powerStrike.transform.rotation);
            strike.GetComponent<Animator>().Play("powerStrike");
            Destroy(strike, 1.5f);
        }
    }

    public void PlaySaberAttack()
    {
        GameObject saber = Instantiate(saberAttack, new Vector3(50, 15, 30), saberAttack.transform.rotation);
        saber.GetComponent<Animator>().Play("saberAttack");
        Destroy(saber, 1.0f);
    }

    public void PlayPierce()
    {
        GameObject spawnedPierce = Instantiate(pierce, new Vector3(51, 10, 85), pierce.transform.rotation);
        spawnedPierce.GetComponent<Animator>().Play("pierce");
        Destroy(spawnedPierce, 2.0f);
    }
}
