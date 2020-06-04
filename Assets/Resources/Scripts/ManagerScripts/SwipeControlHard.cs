using UnityEngine;

public class SwipeControlHard : MonoBehaviour
{
    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private readonly float minSwipeDist = 30.0f;
    private readonly float maxSwipeTime = 0.5f;
    
    private Vector3 touchPosWorld;
    private Vector2 touchPosWorld2D;
    private RaycastHit2D hitInformation;

    private readonly string player = "Player", wall = "Wall", leftLeg = "LeftLeg", rightLeg = "RightLeg";

    private void Update()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0.0f)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                    hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
                    if (hitInformation.collider != null)
                    {
                        GameObject touchedObject = hitInformation.transform.gameObject;
                        if (touchedObject.CompareTag(player)) {
                            isSwipe = true;
                            fingerStartTime = Time.time;
                            fingerStartPos = touch.position;
                        }
                    }
                    break;
                case TouchPhase.Canceled:
                    isSwipe = false;
                    Reset();
                    break;
                case TouchPhase.Ended:
                    float gestureTime = Time.time - fingerStartTime;
                    float gestureDist = (touch.position - fingerStartPos).magnitude;
                    if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                    {
                        touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
                        SwipeType swipeType = SwipeType.NONE;
                        if (hitInformation.collider != null)
                        {
                            GameObject touchedObject = hitInformation.transform.gameObject;
                            if (touchedObject.CompareTag(wall)) swipeType = SwipeType.UP;
                            else if (touchedObject.CompareTag(leftLeg)) swipeType = SwipeType.LEFT;
                            else if (touchedObject.CompareTag(rightLeg)) swipeType = SwipeType.RIGHT;
                        }
                        LevelManager.Instance.SetSwipe(swipeType);
                    }
                    Reset();
                    break;
            }
        }
    }

    private void Reset()
    {
        fingerStartTime = 0.0f;
        fingerStartPos = Vector2.zero;
    }
}
