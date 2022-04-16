using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnManagerScript : MonoBehaviour
{
    //Enemy GameObject
    [SerializeField]
    private GameObject[] Enemies;
    [SerializeField]
    private GameObject[] BossEnemies;
    [SerializeField]
    private GameObject[] powerUPS;
    [SerializeField]
    private Text _RoundText;
    [SerializeField]
    private Text _GameWonText;

    private int _round = 1;

    private PlayerScript _player;

    // Start is called before the first frame update
    void Start()
    {
        _RoundText.text = "Round: " + _round.ToString();

        _GameWonText.gameObject.SetActive(false);

        _player = GameObject.Find("Player").GetComponent<PlayerScript>();

        StartCoroutine(SpawnEnemy());
        StartCoroutine(PowerUp());
        StartCoroutine(Rounds());
    }

    // Update is called once per frame
    void Update()
    {
        if (_round == 2)
        {
            _RoundText.text = "Round: " + _round.ToString();
        }
        if (_round == 3)
        {
            _RoundText.text = "Round: " + _round.ToString();
        }
    }

    IEnumerator SpawnEnemy()
    {
        int randomEnemyLevel = 0;
        while (true)
        {
            switch (_round)
            {
                case 1:
                    yield return new WaitForSeconds(3);
                    randomEnemyLevel = Random.Range(0, 1);
                    break;
                case 2:
                    yield return new WaitForSeconds(2.5f);
                    randomEnemyLevel = Random.Range(0, 2);
                    break;
                case 3:
                    yield return new WaitForSeconds(2);
                    randomEnemyLevel = Random.Range(0, 3);
                    break;
            }
            Instantiate(Enemies[randomEnemyLevel], new Vector3(Random.Range(-15f, 15f), 13f, 0f), Quaternion.identity);
        }
    }

    IEnumerator PowerUp()
    {
        int randomPowerup = 0;
        while (true)
        {
            switch (_round)
            {
                case 1:
                    yield return new WaitForSeconds(Random.Range(5f, 6f));
                    randomPowerup = Random.Range(0, 1);
                    break;
                case 2:
                    yield return new WaitForSeconds(Random.Range(4f, 5f));
                    randomPowerup = Random.Range(0, 2);
                    break;
                case 3:
                    yield return new WaitForSeconds(Random.Range(3f, 4f));
                    randomPowerup = Random.Range(0, 3);
                    break;
            }
            Instantiate(powerUPS[randomPowerup], new Vector3(Random.Range(-15f, 15f), 13f, 0f), Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        StopAllCoroutines();
    }

    IEnumerator Rounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            BossEnemies[0].SetActive(true);
            yield return new WaitUntil(checkedBossLevel1);
            _round += 1;
            yield return new WaitForSeconds(10);
            BossEnemies[1].SetActive(true);
            yield return new WaitUntil(checkedBossLevel2);
            _round += 1;
            yield return new WaitForSeconds(10);
            BossEnemies[2].SetActive(true);
            yield return new WaitUntil(checkedBossLevel3);
            GameWon();
        }
    }

    bool checkedBossLevel1()
    {
        if (BossEnemies[0] == null)
        {
            return true;
        }
        return false;
    }

    bool checkedBossLevel2()
    {
        if (BossEnemies[1] == null)
        {
            return true;
        }
        return false;
    }

    bool checkedBossLevel3()
    {
        if (BossEnemies[2] == null)
        {
            return true;
        }
        return false;
    }

    private void GameWon()
    {
        StopAllCoroutines();
        _GameWonText.gameObject.SetActive(true);
        StartCoroutine(GameWonFlickerRoutine());
        _player.GameIsWon();
    }

    IEnumerator GameWonFlickerRoutine()
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                _GameWonText.text = "Congratulations\n   Game Won";
                yield return new WaitForSeconds(0.5f);
                _GameWonText.text = "";
                yield return new WaitForSeconds(0.5f);
            }
            StopAllCoroutines();
            SceneManager.LoadScene(0);
        }
    }
}
