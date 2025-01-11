using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public PoolManager pool;
    public static GameManager instance;
    public Nexus nexus;

    public int playerCharacterIndex;
    public int computerCharacterIndex;
    [Header("Player Character")]
    public GameObject playerCharacter;
    [Header("Computer Character")]
    public GameObject enemyCharacter;

    [Header("Nexus Object")]
    public GameObject playerNexus;
    public GameObject enemyNexus;

    [Header("Units Object")]
    public List<GameObject> playerUnits = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();

    [Header("Character Sprites")]
    public Sprite[] CharacterSprites;
    public Sprite[] NexusSprites;

    

    void Awake()
    {
        instance = this;
        playerCharacterIndex = PlayerPrefs.GetInt("PlayerCharacter");
        computerCharacterIndex = PlayerPrefs.GetInt("ComputerCharacter");
        Debug.Log("PlayerCharacter: " + playerCharacterIndex);
        Debug.Log("ComputerCharacter: " + computerCharacterIndex);

        instance = this;
        SetSprite();

    }

    void SetSprite()
    {
        // 플레이어 캐릭터 스프라이트 설정  
        playerCharacter.GetComponent<SpriteRenderer>().sprite = CharacterSprites[playerCharacterIndex];
        enemyCharacter.GetComponent<SpriteRenderer>().sprite = CharacterSprites[computerCharacterIndex];
        playerNexus.GetComponent<SpriteRenderer>().sprite = NexusSprites[playerCharacterIndex];
        enemyNexus.GetComponent<SpriteRenderer>().sprite = NexusSprites[computerCharacterIndex];
        

    }

    private void EndGame()
    {
        //_GameState = EGameState.End;
        //_combatUI.ShowEndScreen(_scoreDict[EUnitGroup.Allay], _scoreDict[EUnitGroup.Enemy]);
        //_audioSource.Stop();
    }
}