using UnityEngine;
public enum SwipeType
{
    NONE,
    LEFT,
    UP,
    RIGHT,
    DOWN
}
public class SwipeControl : MonoBehaviour
{
    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private readonly float minSwipeDist = 30.0f;
    private readonly float maxSwipeTime = 0.5f;

    private int choice;

    private void Start()
    {
        if (gameObject.GetComponent<LevelManager>()){
            choice = 0;
        }
        else if (gameObject.GetComponent<ChooseLevel>()) {
            choice = 1; 
        }
        else if (gameObject.GetComponent<CustomizationScene>())
        {
            choice = 2;
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0.0f) {
            Touch touch = Input.touches[0];
            switch (touch.phase) {
                case TouchPhase.Began:
                    isSwipe = true;
                    fingerStartTime = Time.time;
                    fingerStartPos = touch.position;
                    break;
                case TouchPhase.Canceled:
                    isSwipe = false;
                    Reset();
                    break;
                case TouchPhase.Ended:
                    float gestureTime = Time.time - fingerStartTime;
                    float gestureDist = (touch.position - fingerStartPos).magnitude;
                    if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
                        Vector2 direction = touch.position - fingerStartPos;
                        SwipeType swipeType=SwipeType.NONE;
                        if (Mathf.Abs(direction.normalized.x) > 0.9){
                            //horizontal
                            swipeType = SwipeHorizontal(direction) ? SwipeType.RIGHT : SwipeType.LEFT;
                        }
                        else if (Mathf.Abs(direction.normalized.y) > 0.9){
                            //vertical
                            swipeType = SwipeVertical(direction) ? SwipeType.UP : SwipeType.DOWN;
                        }
                        switch (choice)
                        {
                            case 0: LevelManager.Instance.SetSwipe(swipeType); break;
                            case 1: ChooseLevel.Instance.SetSwipe(swipeType); break;
                            case 2: CustomizationScene.Instance.SetSwipe(swipeType); break;
                            default:break;
                        }
                    }
                    Reset();
                    break;
            }
        }
    }

    private bool SwipeHorizontal(Vector2 direction) {
        return Mathf.Sign(direction.x) > 0;
    }

    private bool SwipeVertical(Vector2 direction)
    {
        return Mathf.Sign(direction.y) > 0;
    }

    private void Reset()
    {
        fingerStartTime = 0.0f;
        fingerStartPos = Vector2.zero;
    }
}
