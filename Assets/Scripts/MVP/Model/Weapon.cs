using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private CharacterPresenter _owner;
    private GameObject _target;
    private float damage;
    void OnEnable()
    {
        if (_owner.CompareTag(StringConstant.CHARACTER.PLAYER))
        {
            _target = GameObject.FindGameObjectWithTag(StringConstant.CHARACTER.ENEMY);
        }
        else
        {
            _target = GameObject.FindGameObjectWithTag(StringConstant.CHARACTER.PLAYER);
        }
    }

    void Start()
    {
        damage = _owner.characterData.attackDamage;
        this.gameObject.SetActive(false);
    }
    public void ThrowWeapon()
    {
        this.gameObject.SetActive(true);
        transform.DOMove(_target.transform.position, 0.1f).SetEase(Ease.Linear);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_target.tag))
        {
            CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
            character.TakeDamage(damage, GameManager.AnimationState.MINIHURT);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(_target.tag))
        {
            CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
            character.EndTakingDamage(GameManager.AnimationState.MINIHURT);
            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.UNPAUSE_TIMER);
            transform.DOKill();
            this.gameObject.SetActive(false);
            _owner.GetComponent<Animator>().ResetTrigger(GameManager.AnimationState.ATTACK);
        }
    }


}
