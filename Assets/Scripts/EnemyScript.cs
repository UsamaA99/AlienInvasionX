using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private float _speed = 4.0f;
    [SerializeField]
    private int EnemyLevel;
    private bool MovePartial = false;
    private bool MoveNegX = false;
    private bool MovePosX = false;
    [SerializeField]
    private GameObject _laserPrefabs;

    private PlayerScript _player;

    [SerializeField]
    private GameObject _EnemyExplode;

    private Animator _anim;
    private SpriteRenderer _sprite;
    private EnemyScript _script;
    private CapsuleCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();

        _anim = _EnemyExplode.GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _script = GetComponent<EnemyScript>();
        _collider = GetComponent<CapsuleCollider2D>();

        StartCoroutine(AttackRoutine());

        if (EnemyLevel == 2)
        {
            StartCoroutine(MovePartialCoroutine());
        }
        if (EnemyLevel == 3)
        {
            StartCoroutine(MoveXCoroutine());
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (EnemyLevel)
        {
            case 1:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            case 2:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    
                if (MovePartial == true)
                {
                    if (transform.position.x > -15 && transform.position.x < 15)
                    {
                        transform.Translate(Vector3.right * _speed * Time.deltaTime);
                    }
                }
                break;
            case 3:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);

                if (MovePosX == true)
                {
                    if (transform.position.x > -15 && transform.position.x < 15)
                    {
                        transform.Translate(Vector3.right * _speed * Time.deltaTime);
                    }
                }
                else if (MoveNegX == true)
                {
                    if (transform.position.x > -15 && transform.position.x < 15)
                    {
                        transform.Translate(-(Vector3.right) * _speed * Time.deltaTime);
                    }
                }
                break;
        }

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(Random.Range(-15f,15f),13f,0f);
        }
    }

    IEnumerator MovePartialCoroutine()
    {
        while (true)
        {
            MovePartial = true;
            yield return new WaitForSeconds(1);
            MovePartial = false;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator MoveXCoroutine()
    {
        while (true)
        {
            MovePosX = true;
            yield return new WaitForSeconds(1.5f);
            MovePosX = false;
            yield return new WaitForSeconds(2.5f);
            MoveNegX = true;
            yield return new WaitForSeconds(1.5f);
            MoveNegX = false;
            yield return new WaitForSeconds(2.5f);

        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Laser")
        {
            if (_player != null)
            {
                _player.Score(EnemyLevel);
            }

            StopAllCoroutines();
            StartCoroutine(Sprite());
            _script.enabled = false;
            _collider.enabled = false;

            _EnemyExplode.SetActive(true);
            _anim.SetTrigger("OnEnemyDeath");

            Destroy(this.gameObject, 2.8f);
            Destroy(collider.gameObject);
        }

        if (collider.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(Sprite());
            _script.enabled = false;
            _collider.enabled = false;

            _EnemyExplode.SetActive(true);
            _anim.SetTrigger("OnEnemyDeath");

            Destroy(this.gameObject, 2.8f);

            PlayerScript Player = collider.transform.GetComponent<PlayerScript>();

            if (Player != null)
            {
                Player.Damage();
            }
        }
    }

    private void Attack() 
    {
        switch (EnemyLevel)
        {
            case 1:
                Instantiate(_laserPrefabs, new Vector3(transform.position.x - 6.86f, transform.position.y - 5.58f, 0), Quaternion.identity);
                break;
            case 2:
                Instantiate(_laserPrefabs, new Vector3(transform.position.x - 0.83f, transform.position.y - 3.4f, 0), Quaternion.identity);
                break;
            case 3:
                Instantiate(_laserPrefabs, new Vector3(transform.position.x + 5.85f, transform.position.y + 3.12f, 0), Quaternion.identity);
                break;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
            Attack();
        }
    }

    private IEnumerator Sprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _sprite.enabled = false;
            break;
        }
    }
}
