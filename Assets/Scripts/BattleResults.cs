using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResults : MonoBehaviour
{
    public Image HealthFill;
    public Image ManaFill;
    public Text HealthButtonText;
    public Text ManaButtonText;

    private const float OrigRewardHealthFillWidth = 285.0f;
    private const float OrigRewardManaFillWidth = 285.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRewards(float HealthReward, float ManaReward)
    {
        float newWidth = OrigRewardHealthFillWidth * HealthReward / 100f;
        HealthFill.rectTransform.sizeDelta = new Vector2(newWidth, HealthFill.rectTransform.sizeDelta.y);
        HealthButtonText.text = string.Format("Reward {000:0}", HealthReward);

        newWidth = OrigRewardManaFillWidth * ManaReward / 100f;
        ManaFill.rectTransform.sizeDelta = new Vector2(newWidth, ManaFill.rectTransform.sizeDelta.y);
        ManaButtonText.text = string.Format("Reward {000:0}", ManaReward);
    }
}
