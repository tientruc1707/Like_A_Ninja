using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private CharacterPresenter _owner;
    private float damage = 10f;
    public void ThrowWeapon()
    {
        this.gameObject.SetActive(true);
        if (_owner.CompareTag(StringConstant.CHARACTER.PLAYER))
        {
            GameObject target = GameObject.FindGameObjectWithTag(StringConstant.CHARACTER.ENEMY);
            transform.position = Vector2.Lerp(this.transform.position, target.transform.position, 2f);
        }
        else
        {
            GameObject target = GameObject.FindGameObjectWithTag(StringConstant.CHARACTER.PLAYER);
            Vector2.Lerp(this.transform.position, target.transform.position, 2f);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(StringConstant.CHARACTER.PLAYER) ||
            collision.CompareTag(StringConstant.CHARACTER.ENEMY))
        {
            CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
            character.TakeDamage(damage, GameManager.AnimationState.MINIHURT);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        CharacterPresenter character = collision.GetComponent<CharacterPresenter>();
        character.EndTakingDamage(GameManager.AnimationState.MINIHURT);
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.UNPAUSE_TIMER);
        this.gameObject.SetActive(false);
    }


}
