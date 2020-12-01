using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSignifiers : MonoBehaviour
{
    public bool BattleZoneEnabled;
    public bool ManaZoneEnabled;
    public int MaxTimesZonesCanShow;
    public float TimeUntilZonesShows;
    public int MaxTimesMZCanShow;
    public int TimesManaUnsedBeforeShow;
    public Image BattleZoneSignifier;
    public Image ManaZoneSignifier;
    public int TimesZonesHaveShown;
    public int TimesManaShown;
    public int ManaUnusedCount;

    private float m_ZonesToShowTmr;
    private Animator m_BZAnimator;
    private bool m_BZAnimPlaying;
    private Animator m_MZAnimator;
    private bool m_MZAnimPlaying;
    enum SigStateType { BEGIN_TURN, END_TURN, UNKNOWN };
    private SigStateType m_SignifiersState = SigStateType.UNKNOWN;
    private bool m_ManaAllowed;
    private bool m_OnlyManaSigShowing;

    // Start is called before the first frame update
    void Start()
    {
        // In case someone has forgoton to set the property
        if (MaxTimesZonesCanShow == 0)
        {
            MaxTimesZonesCanShow = 3;
        }

        TimesZonesHaveShown = 0;
        m_ZonesToShowTmr = 0.0f;
        m_BZAnimPlaying = false;
        TimesManaShown = 0;
        m_MZAnimPlaying = false;
        m_ManaAllowed = false;
        ManaUnusedCount = 0;
        m_OnlyManaSigShowing = false;

        m_BZAnimator = BattleZoneSignifier.GetComponent<Animator>();
        m_MZAnimator = ManaZoneSignifier.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // checked if All are enabled
        if (BattleZoneEnabled == true)
        { 
            // Process the mana signifier algorithim
            ProcessZoneSignifiers();
        }

        // checked if mana enabled
        if (ManaZoneEnabled == true)
        {
            // Process the mana signifier algorithim
            ProcessManaArrowSignifier();
        }
    }

    void ProcessZoneSignifiers()
    {
        // check if we have shown all the maximum times
        if (TimesZonesHaveShown < MaxTimesZonesCanShow)
        {
            if (m_SignifiersState == SigStateType.BEGIN_TURN && m_BZAnimPlaying == false)
            {
                // check if the waiting time has expired for all
                m_ZonesToShowTmr += Time.deltaTime;

                if (m_ZonesToShowTmr > TimeUntilZonesShows)
                {
                    // All expired so turn on all signifier animation
                    m_BZAnimator.enabled = true;
                    m_BZAnimator.Play("BattleZone_Signifier");
                    m_BZAnimPlaying = true;
                    BattleZoneSignifier.gameObject.SetActive(true);
                    if (m_ManaAllowed == true)
                    {
                        if (m_OnlyManaSigShowing == true)
                        {
                            // take over control by syncing what mana woud do to shut off and then take control
                            m_MZAnimator.StopPlayback();
                            m_MZAnimator.enabled = false;
                            m_MZAnimPlaying = false;
                            ManaZoneSignifier.gameObject.SetActive(false);
                            TimesManaShown++;
                            m_OnlyManaSigShowing = false;
                        }
                        m_MZAnimator.enabled = true;
                        m_MZAnimator.Play("ManaZone_Signifier");
                        m_MZAnimPlaying = true;
                        ManaZoneSignifier.gameObject.SetActive(true);
                    }
                    m_ZonesToShowTmr -= TimeUntilZonesShows;
                }
            }
            // Its playing or now end of turn so watch for the termination
            else if (   m_SignifiersState == SigStateType.END_TURN 
                     && (m_BZAnimPlaying == true || m_MZAnimPlaying == true))
            {
                if (m_BZAnimPlaying == true)
                {
                    m_BZAnimator.StopPlayback();
                    m_BZAnimator.enabled = false;
                    m_BZAnimPlaying = false;
                    BattleZoneSignifier.gameObject.SetActive(false);
                    TimesZonesHaveShown++;
                }

                if (m_MZAnimPlaying == true)
                {
                    m_MZAnimator.StopPlayback();
                    m_MZAnimator.enabled = false;
                    m_MZAnimPlaying = false;
                    ManaZoneSignifier.gameObject.SetActive(false);
                }
            }
        }
    }

    void ProcessManaArrowSignifier()
    {
        // check if we have shown all the maximum times
        if (    ManaUnusedCount >= TimesManaUnsedBeforeShow 
            &&  TimesManaShown < MaxTimesMZCanShow)
        {
            if (   m_SignifiersState == SigStateType.BEGIN_TURN 
                && m_MZAnimPlaying == false && m_ManaAllowed == true)
            {
                // Mana expired so turn on Mana signifier animation
                m_MZAnimator.enabled = true;
                m_MZAnimator.Play("ManaZone_Signifier");
                m_MZAnimPlaying = true;
                ManaZoneSignifier.gameObject.SetActive(true);
                ManaUnusedCount = 0;
                m_OnlyManaSigShowing = true;
            }
            // Its playing or now end of turn so watch for the termination
            else if (   m_SignifiersState == SigStateType.END_TURN 
                     && m_MZAnimPlaying == true && m_OnlyManaSigShowing == true)
            {
                m_MZAnimator.StopPlayback();
                m_MZAnimator.enabled = false;
                m_MZAnimPlaying = false;
                ManaZoneSignifier.gameObject.SetActive(false);
                TimesManaShown++;
                m_OnlyManaSigShowing = false;
            }
        }
    }

    public void StartTurn(bool ManaAvailable)
    {
        m_ManaAllowed = ManaAvailable;
        m_SignifiersState = SigStateType.BEGIN_TURN;
        m_ZonesToShowTmr = 0f;
    }

    public void EndTurn(bool ManaWasUsed)
    {
        if (m_ManaAllowed == true && ManaWasUsed == false)
        {
            ManaUnusedCount++;
        }
        else // (ManaWasUsed == true)
        {
            ManaUnusedCount = 0;
        }
        m_ManaAllowed = false;
        m_SignifiersState = SigStateType.END_TURN;
    }
}
