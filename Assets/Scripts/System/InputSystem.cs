using UnityEngine;

public class InputSystem : Singleton<InputSystem>
{
    private Vector2 m_TouchStartPosition;
    private bool m_IsTouching;
    public float swipeThreshold = 50f;

    public delegate void OnSwipeDetected(Vector2 startPos, Vector2 direction);
    public event OnSwipeDetected SwipeDetected;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    m_TouchStartPosition = touch.position;
                    m_IsTouching = true;
                    break;
                case TouchPhase.Ended:
                    if (m_IsTouching)
                    {
                        Vector2 touchEndPosition = touch.position;
                        DectectSwipe(m_TouchStartPosition, touchEndPosition);
                    }
                    m_IsTouching = false; // Reset on end
                    break;
                case TouchPhase.Canceled:
                    m_IsTouching = false; // Reset on end or cancel
                    break;
            }
        }
    }

    private void DectectSwipe(Vector2 startPos, Vector2 endPos)
    {
        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;
        if (distance >= swipeThreshold)
        {
            direction.Normalize();
            Vector2 dir = Vector2.zero;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                dir = direction.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                dir = direction.y > 0 ? Vector2.up : Vector2.down;
            }
            SwipeDetected?.Invoke(startPos, dir);
        }
    }

    public void ResetTouch()
    {
        m_IsTouching = false;
    }

}
