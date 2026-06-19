using TMPro;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private float _shieldDuration = 5f;
    [SerializeField] private float _cooldownDuration = 10f;

    public enum SheildState { Ready, Active, Cooldown }
    private SheildState _state = SheildState.Ready;

    private float _timer;

    private GameObject _shieldInstance;

    public float sheildHeightOffset = 0.7f;

    private Animator _sheildAnimator;

    public float sheildDamage = 40f;
    public TextMeshProUGUI sheildTimerText;
    public SheildState state => _state;

    public static PlayerShield instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Update()
    {
        HandleInput();
        RunTimer();

        if (_state is not SheildState.Ready)
        {
            sheildTimerText.gameObject.SetActive(true);
            DisplaySheildTimer(_state);
        }
        else
            sheildTimerText.gameObject.SetActive(false);
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _state == SheildState.Ready)
        {
            ActivateShield();
            _sheildAnimator.SetTrigger("sheildActivate");
            AudioManager.Instance.StartShieldSound();
        }
    }

    private void ActivateShield()
    {
        Vector3 sheildPos = transform.position + transform.up * sheildHeightOffset;

        _shieldInstance = Instantiate(_shieldPrefab, sheildPos, Quaternion.identity, transform);

        _sheildAnimator = _shieldInstance.GetComponent<Animator>();

        SetTimer(_shieldDuration);
        _state = SheildState.Active;

    }

    private void SetTimer(float duration)
    {
        _timer = duration;
    }

    private void RunTimer()
    {
        if (_state == SheildState.Ready) return;
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _timer = 0;
            if (_state == SheildState.Active)
            {
            DeactivateShield();
            AudioManager.Instance.StopShieldSound();
            }
            else if (_state == SheildState.Cooldown) _state = SheildState.Ready;
        }
    }

    private void DeactivateShield()
    {
      if (_shieldInstance != null)
      {
        _sheildAnimator.SetTrigger("sheildDeactivate");
        Invoke("DestroySheild", 1f);
      }

      _state = SheildState.Cooldown;

      SetTimer(_cooldownDuration);
    }

    public void DisplaySheildTimer(SheildState s)
    {
        sheildTimerText.text = Mathf.RoundToInt(_timer).ToString();
        if (s is SheildState.Active)
        {
            sheildTimerText.color = Color.blueViolet;
        }
        else
        {
            sheildTimerText.color = Color.black;
        }
    }

    private void DestroySheild()
      {
      Destroy(_shieldInstance);
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Enemy"))
        {
            obj.GetComponent<SharkHealth>().TakeDamage(sheildDamage);
        }
    }

}
