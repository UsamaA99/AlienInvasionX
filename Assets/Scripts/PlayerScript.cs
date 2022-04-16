using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float _moveSpeed = 7f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 0.3f;
    private float _nextFire = -1f;

    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject[] _Wings;

    [SerializeField]
    private AudioClip _LaserSoundClip;

    private AudioSource _audioSource;

    private UIManagerScript _UIManager;

    private SpawnManagerScript _spawnManager;

    private bool _GameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManagerScript>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManagerScript>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        transform.position = new Vector3(0f,-3.0f,0f);

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Player audio has not been assigned!");
        }
        else
        {
            _audioSource.clip = _LaserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKey(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        //Movement of Player
        //Moves up and down along y axis //Up/Down
        transform.Translate(Vector3.up * Time.deltaTime * Input.GetAxis("Vertical") * _moveSpeed);
        //Moves Left and right along x Axis //Left/Right
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * _moveSpeed);

        //Restraining Player movements on y-axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -6.8f, -3.0f), 0);
        
        //Restraining Player movements on x-axis
        if (transform.position.x < -18)
        {
            transform.position = new Vector3(18, transform.position.y, 0);
        }
        else if (transform.position.x > 18)
        {
            transform.position = new Vector3(-18, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
        }
        else {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.03f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _fireRate += 0.2f;
        StartCoroutine(TripleShotDownCoroutine());
    }

    IEnumerator TripleShotDownCoroutine()
    {
        yield return new WaitForSeconds(3);
        _isTripleShotActive = false;
        _fireRate -= 0.2f;
    }

    public void SpeedBoostActive()
    {
        _moveSpeed = 11.0f;
        StartCoroutine(SpeedBoostDownCoroutine());
    }

    IEnumerator SpeedBoostDownCoroutine()
    {
        yield return new WaitForSeconds(5);
        _moveSpeed = 7.0f;
    }

    public void ShieldActive()
    {
        _shieldVisualizer.SetActive(true);
        _isShieldActive = true;
        StartCoroutine(ShieldCoroutine());
    }

    IEnumerator ShieldCoroutine()
    {
        yield return new WaitForSeconds(4);
        _shieldVisualizer.SetActive(false);
        _isShieldActive = false;
    }

    public void Damage()
    {
        if (!_isShieldActive && !_GameWon)
        {
            _lives--;
            _UIManager.UpdatePlayerLives(_lives);

            if (_lives == 2)
            {
                _Wings[0].SetActive(true);
            }
            else if (_lives == 1)
            {
                _Wings[1].SetActive(true);
            }
            else if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }

    public void Score(int _enemyLevel = 0)
    {
        switch (_enemyLevel)
        {
            case 1:
                _score += 10;
                break;
            case 2:
                _score += 20;
                break;
            case 3:
                _score += 30;
                break;
            case 4:
                _score += 100;
                break;
            case 5:
                _score += 200;
                break;
            case 6:
                _score += 300;
                break;
        }
        if (_UIManager != null)
        {
            _UIManager.UpdateScore(_score);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "EnemyLaser")
        {
            Damage();
            Destroy(collider.gameObject);
        }
    }

    public void GameIsWon()
    {
        _GameWon = true;
    }
}
