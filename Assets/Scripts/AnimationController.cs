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
    public GameObject shield;
    public GameObject run;

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlaySlash();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayDaggerThrow();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayHeal();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlaySiphon();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayPowerStrike();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlaySaberAttack();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayPierce();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayShield();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayRun();
        }
    }

    public void PlaySlash()
    {
        GameObject spawnedSlash = Instantiate(slash, new Vector3(-54, 84, 70), slash.transform.rotation);
        spawnedSlash.GetComponent<Animator>().Play("slash redo");
        Destroy(spawnedSlash, 1.0f);
    }

    public void PlayDaggerThrow()
    {
        GameObject daggers = Instantiate(daggerThrow, this.transform.position, daggerThrow.transform.rotation);
        daggers.GetComponent<Animator>().Play("singleDaggerThrow redo");
        Destroy(daggers, 1.5f);
    }

    public void PlayHeal()
    {
        GameObject potion = Instantiate(potionHeal, new Vector3(50, 0, 30), potionHeal.transform.rotation);
        potion.GetComponent<Animator>().Play("heal redo");
        Destroy(potion, 2.25f);
    }

    public void PlaySiphon()
    {
        GameObject hook = Instantiate(siphon, new Vector3(50, 15, 30), siphon.transform.rotation);
        hook.GetComponent<Animator>().Play("siphon redo");
        Destroy(hook, 1.5f);
    }

    public void PlayPowerStrike()
    {
        if (Player != null)
        {
            if (Player.Mana >= 50 && Player.Mana < 100)
            {
                GameObject strike = Instantiate(powerStrike, new Vector3(50, 87, 70), powerStrike.transform.rotation);
                strike.GetComponent<Animator>().Play("powerStrike redo");
                Destroy(strike, 2.5f);
            }
            else
            {
                GameObject strike = Instantiate(powerStrikeNoMana, new Vector3(50, 87, 70), powerStrike.transform.rotation);
                strike.GetComponent<Animator>().Play("powerStrike redo");
                Destroy(strike, 1.5f);
            }
        } else
        {
            GameObject strike = Instantiate(powerStrike, new Vector3(50, 87, 70), powerStrike.transform.rotation);
            strike.GetComponent<Animator>().Play("powerStrike redo");
            Destroy(strike, 2.5f);
        }
    }

    public void PlaySaberAttack()
    {
        GameObject saber = Instantiate(saberAttack, new Vector3(50, 15, 30), saberAttack.transform.rotation);
        saber.GetComponent<Animator>().Play("saberAttack redo");
        Destroy(saber, 1.0f);
    }

    public void PlayPierce()
    {
        GameObject spawnedPierce = Instantiate(pierce, new Vector3(50, 110, 70), pierce.transform.rotation);
        spawnedPierce.GetComponent<Animator>().Play("pierce redo");
        Destroy(spawnedPierce, 2.0f);
    }

    public void PlayShield()
    {
        GameObject spawnedShield = Instantiate(shield, new Vector3(50, 0, 30), potionHeal.transform.rotation);
        spawnedShield.GetComponent<Animator>().Play("shield");
        Destroy(spawnedShield, 2.25f);
    }

    public void PlayRun()
    {
        GameObject playRun = Instantiate(run, new Vector3(50, 0, 30), potionHeal.transform.rotation);
        playRun.GetComponent<Animator>().Play("run");
        Destroy(playRun, 2.25f);
    }
}
