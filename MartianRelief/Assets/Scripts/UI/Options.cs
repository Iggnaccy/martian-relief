using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Options : MonoBehaviour {

	public Slider playerHp;
	public Slider playerDmg;
	public Slider playerMoveSpeed;
	public Slider playerAttackSpeed;
	public Slider enemyHp;

	public Text playerHpText;
	public Text playerDmgText;
	public Text playerMoveSpeedText;
	public Text playerAttackSpeedText;
	public Text enemyHpText;

	string playerHpBasicText = "Player's <color=red>hp</color>: ";
	string playerDmgBasicText = "Player's <color=red>dmg</color>: ";
	string playerMoveSpeedBasicText = "Player's <color=blue>speed</color>: ";
	string playerAttackSpeedBasicText = "Player's <color=blue>attack speed</color>: ";
	string enemyHpBasicText = "Enemies' <color=green>hp</color>: ";

	public void Start(){

		playerHp.value = Static.playerHp;
		playerDmg.value = Static.playerDmg;
		playerMoveSpeed.value = Static.playerMoveSpeed;
		playerAttackSpeed.value = Static.playerAttackSpeed;
		enemyHp.value = Static.enemyHp;

		playerHp.onValueChanged.AddListener(delegate {onPlayerHpChanged();});
		playerMoveSpeed.onValueChanged.AddListener(delegate {onPlayerMoveSpeedChanged();});
		playerAttackSpeed.onValueChanged.AddListener(delegate {onPlayerAttackSpeedChanged();});
		playerDmg.onValueChanged.AddListener(delegate {onPlayerDmgChanged();});
		enemyHp.onValueChanged.AddListener(delegate {onEnemyHpChanged();});

		playerHpText.text = playerHpBasicText + Static.playerHp.ToString () + "/" + ((int)playerHp.maxValue).ToString();
		playerMoveSpeedText.text = playerMoveSpeedBasicText + Static.playerMoveSpeed.ToString ("F2") + "/" + playerMoveSpeed.maxValue.ToString("F2");
		playerAttackSpeedText.text = playerAttackSpeedBasicText + Static.playerAttackSpeed.ToString ("F2") + "/" + playerAttackSpeed.maxValue.ToString("F2");
		playerDmgText.text = playerDmgBasicText + Static.playerDmg.ToString ("F2") + "/" + playerDmg.maxValue.ToString("F2");
		enemyHpText.text = enemyHpBasicText + Static.enemyHp.ToString ("F2") + "/" + enemyHp.maxValue.ToString("F2");
	}

	public void onPlayerHpChanged(){
		Static.playerHp = (int)playerHp.value;
		playerHpText.text = playerHpBasicText + Static.playerHp.ToString () + "/" + ((int)playerHp.maxValue).ToString();
	}

	public void onPlayerMoveSpeedChanged(){
		Static.playerMoveSpeed = playerMoveSpeed.value;
		playerMoveSpeedText.text = playerMoveSpeedBasicText + Static.playerMoveSpeed.ToString ("F2") + "/" + playerMoveSpeed.maxValue.ToString("F2");
	}

	public void onPlayerAttackSpeedChanged(){
		Static.playerAttackSpeed = playerAttackSpeed.value;
		playerAttackSpeedText.text = playerAttackSpeedBasicText + Static.playerAttackSpeed.ToString ("F2") + "/" + playerAttackSpeed.maxValue.ToString("F2");
	}

	public void onPlayerDmgChanged(){
		Static.playerDmg = playerDmg.value;
		playerDmgText.text = playerDmgBasicText + Static.playerDmg.ToString ("F2") + "/" + playerDmg.maxValue.ToString("F2");
	}

	public void onEnemyHpChanged(){
		Static.enemyHp = enemyHp.value;
		enemyHpText.text = enemyHpBasicText + Static.enemyHp.ToString ("F2") + "/" + enemyHp.maxValue.ToString("F2");
	}

	public void onButtonGoBack(){
		Application.LoadLevel("MainMenuScene");
	}
}

