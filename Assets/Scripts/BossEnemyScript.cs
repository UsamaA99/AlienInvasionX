using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemyScript : MonoBehaviour
{
    private float _speed = 4.0f;
    [SerializeField]
    private int BossEnemyLevel;
    [SerializeField]
    private int BossEnemyLives;
    [SerializeField]
    private Sprite[] _BossEnemyLivesSprites;
    [SerializeField]
    private Image _BossLivesImage;
    
    private bool MoveNegX = false;
    private bool MovePosX = false;
    [SerializeField]
    private GameObject _laserPrefabs;
    [SerializeField]
    private GameObject _explosionPrefab;

    private PlayerScript _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();

        _BossLivesImage.sprite = _BossEnemyLivesSprites[BossEnemyLives-1];

        StartCoroutine(AttackRoutine());
        StartCoroutine(MoveXCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 6.5)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            _BossLivesImage.transform.Translate(Vector3.down * 5f * _speed * Time.deltaTime);
        }
        if (MovePosX == true && transform.position.y < 7.5f)
        {
            if (transform.position.x > -8.5f && transform.position.x < 8.5f)
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                _BossLivesImage.transform.Translate(Vector3.right * 30f * _speed * Time.deltaTime);
            }
        }
        else if (MoveNegX == true && transform.position.y < 7.5f)
        {
            if (transform.position.x > -8.5f && transform.position.x < 8.5f)
            {
                transform.Translate(-(Vector3.right) * _speed * Time.deltaTime);
                _BossLivesImage.transform.Translate(-(Vector3.right) * 30f * _speed * Time.deltaTime);
            }
        }
    }

    IEnumerator MoveXCoroutine()
    {
        while (true)
        {
            MovePosX = true;
            yield return new WaitForSeconds(3.5f);
            MovePosX = false;
            MoveNegX = true;
            yield return new WaitForSeconds(3.5f);
            MoveNegX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Laser")
        {
            BossEnemyLives -= 1;

            if (BossEnemyLives <= 0)
            {
                if (_player != null)
                {
                    _player.Score(BossEnemyLevel);
                }

                _BossLivesImage.rectTransform.anchoredPosition = new Vector3(0, 15, 0);
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject, 0.2f);
            }
            else
            {
                _BossLivesImage.sprite = _BossEnemyLivesSprites[BossEnemyLives-1];
            }
            Destroy(collider.gameObject);
        }
    }

    private void Attack()
    {
        switch (BossEnemyLevel)
        {
            case 4:
                Instantiate(_laserPrefabs, new Vector3(transform.position.x + 5.45f, transform.position.y - 0.25f, 0), Quaternion.identity);
                break;
            case 5:
                Instantiate(_laserPrefabs, new Vector3(transform.position.x + 3.6f, transform.position.y + 8f, 0), Quaternion.identity);
                break;
            case 6:
                Instantiate(_laserPrefabs, new Vector3(transform.position.x - 0.1f, transform.position.y + 0.15f, 0), Quaternion.identity);
                break;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 2));
            Attack();
        }
    }
}
