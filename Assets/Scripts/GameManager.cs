using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public PoolManager pool;
    public static GameManager instance;
    public Nexus nexus;

    public int playerCharacterIndex;
    public int computerCharacterIndex;
    
    [Header("Game Control")]
    public float gameTime;
    public float firstAngryTime;
    public float secondAngryTime;
    public AngryEffect angryEffect;
    public GameObject playerNexusRegion;

    [Header("Player Character")]
    public GameObject playerCharacter;
    public int playerKillCount;
    public int maxPlayerKillCount;

    [Header("Computer Character")]
    public GameObject enemyCharacter;
    public int enemyKillCount;
    public int maxEnemyKillCount;

    [Header("Nexus Object")]
    public GameObject playerNexus;
    public GameObject enemyNexus;
    public int playerNexusInteractionCount = 0;
    public int enemyNexusInteractionCount = 0;

    [Header("Units Object")]
    public List<GameObject> playerUnits = new List<GameObject>();
    public int ActivePlayerUnits = 0;
    public List<GameObject> enemyUnits = new List<GameObject>();
    public int ActiveEnemyUnits = 0;

    [Header("Character Sprites")]
    public Sprite[] CharacterSprites;
    public Sprite[] NexusSprites;

    [Header("Game Start Effect")]
    public float startEffectDuration = 3f;
    public bool isGameStarted = false;

    void Awake()
    {
        instance = this;

        playerCharacterIndex = PlayerPrefs.GetInt("PlayerCharacter");
        computerCharacterIndex = PlayerPrefs.GetInt("ComputerCharacter");

        firstAngryTime = 15f;
        secondAngryTime = 30f;
        playerKillCount = 0;
        enemyKillCount = 0;
        maxPlayerKillCount = 0;
        maxEnemyKillCount = 0;

        Debug.Log("PlayerCharacter: " + playerCharacterIndex);
        Debug.Log("ComputerCharacter: " + computerCharacterIndex);

        SetSprite();
    }

    void Start()
    {
        StartCoroutine(GameStartSequence());
    }

    IEnumerator GameStartSequence()
    {
        isGameStarted = false;

        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(startEffectDuration);

        isGameStarted = true;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (playerCharacterIndex == 0) // 캐릭터 빨강일 때
            AddNewUnit();

        if (gameTime > firstAngryTime)
        {
            // Small Angry Active
        }
        else if (gameTime > secondAngryTime)
        {
            // Big Angry Active
        }

        CheckGameEnd();
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    void AddNewUnit()
    {
        if (playerKillCount >= maxPlayerKillCount)
        {
            playerKillCount = 0;
        }
    }

    void SetSprite()
    {
        // 플레이어 캐릭터 스프라이트 설정  
        playerCharacter.GetComponent<SpriteRenderer>().sprite = CharacterSprites[playerCharacterIndex];
        enemyCharacter.GetComponent<SpriteRenderer>().sprite = CharacterSprites[computerCharacterIndex];
        playerNexus.GetComponent<SpriteRenderer>().sprite = NexusSprites[playerCharacterIndex];
        enemyNexus.GetComponent<SpriteRenderer>().sprite = NexusSprites[computerCharacterIndex];
    }

    private void CheckGameEnd()
    {
        if (playerNexus.GetComponent<Nexus>().health <= 0)
        {
            EndGame();
        }
        else if (enemyNexus.GetComponent<Nexus>().health <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (playerNexus.GetComponent<Nexus>().health <= 0)
        {
            PlayerPrefs.SetInt("GameResult", 0);
        } else if (enemyNexus.GetComponent<Nexus>().health <= 0)
        {
            PlayerPrefs.SetInt("GameResult", 1);
        }
        SceneManager.LoadScene("EndingScene");
        //_audioSource.Stop();
    }
}