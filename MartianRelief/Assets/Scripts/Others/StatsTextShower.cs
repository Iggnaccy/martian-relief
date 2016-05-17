using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsTextShower : MonoBehaviour
{
    BasicStats playerStats;
    Text text;

	void Start ()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<BasicStats>();
        text = GetComponent<Text>();
	}
	
	void Update ()
    {
        text.text = "Damage: " + playerStats.damage.GetValue().ToString() + "\n\n Attack Speed: " + playerStats.attackSpeed.GetValue().ToString() + "\n\n Movement Speed: " + playerStats.moveSpeed.GetValue().ToString();
    }
}
