using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Vector2 startTouchPosition; //стартовая позиция касания
    private Vector2 endTouchPosition; //конечная позиция касания
    private bool isDragging = false; //палец на экране?
    private float swipeThreshold = 70f; // эггара кеззигаг1а хьокам д1ахьакха беза моттиг

    private Camera cam;
    private float screenHeight;
    private float screenWidth;
    public float offset = 1f;
    private float leftBound, rightBound, topBound, bottomBound;

    private AudioSource carAudioSource;
    public AudioClip normalMotorSound;
    public AudioClip boostedMotorSound;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float normalSpeed = 1f;
    [SerializeField] private float boostedSpeed = 2f;
    private bool isBoosted = false;

    void Start()
    {
        cam = Camera.main;
        screenHeight = 2f * cam.orthographicSize;
        screenWidth = screenHeight * cam.aspect;

        leftBound = cam.transform.position.x - screenWidth / 2;
        rightBound = cam.transform.position.x + screenWidth / 2;
        bottomBound = cam.transform.position.y - screenHeight / 2;
        topBound = cam.transform.position.y + screenHeight / 2;
        carAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
#if UNITY_EDITOR
        PCInput();

#elif UNITY_ANDROID
        MobileInput();
#endif

        MoveCarForward();
        TeleportToOpposideSide();
        //carAudioSource.PlayOneShot(engineSound);
    }
    void MobileInput()
    {
        if (Input.touchCount > 0) // цкъа хьокхавалар мукъаг1 дале
        {
            Touch touch = Input.GetTouch(0); // хьалхарча хьокхаваларе меттиг хьаэц вай

            if (touch.phase == TouchPhase.Began) // хьокхавалар д1адоладена дале
            {
                startTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended) // хьокхавалар чакхадаьннадале
            {
                endTouchPosition = touch.position;
                isDragging = false;
                ProcessSwipe();
            }
        }
    }
    void PCInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            isDragging = false;
            ProcessSwipe();
        }

    }
    void ProcessSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition; //хокхавалар чакхдалара е из доладалара е юкъара юкъ
        //свайпи йоахал мишт я хьожа вай
        if (swipeDelta.magnitude >= swipeThreshold)
        {
            Vector2 direction = swipeDelta.normalized; //йоахал 1 оттаю вай (нормализовать ю)

            //свайпо де дезар ухаза да
            //CarMovement
            bool IsHorizontalSwipe = Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y);

            if (IsHorizontalSwipe)
            {
                if (swipeDelta.x > 0 && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, 90)) != 180)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                if (swipeDelta.x < 0 && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, -90)) != 180)
                {
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                }
            }
            else
            {
                if (swipeDelta.y > 0 && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, 180)) != 180)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                if (swipeDelta.y < 0 && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, 0)) != 180)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);

                }
            }
        }
        else
        {
            //клик
                if (isBoosted)
                {
                    isBoosted = false;
                    speed = normalSpeed;
                }
                else
                {
                    isBoosted = true;
                    speed = boostedSpeed;
                }
        }
    }
    void MoveCarForward()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (isBoosted)
        {
            if (carAudioSource.clip != boostedMotorSound || !carAudioSource.isPlaying)
            {
                carAudioSource.clip = boostedMotorSound;
                carAudioSource.Play();
            }
        }
        else
        {
            if (carAudioSource.clip != normalMotorSound || !carAudioSource.isPlaying)
            {
                carAudioSource.clip = normalMotorSound;
                carAudioSource.Play();
            }
        }
    }
    void TeleportToOpposideSide()
    {
        Vector2 carPos = transform.position;

        if (carPos.x < leftBound - offset)
        {
            carPos.x = rightBound + offset;
            transform.position = carPos;
        }
        else if (carPos.x > rightBound + offset)
        {
            carPos.x = leftBound - offset;
            transform.position = carPos;
        }
        if (carPos.y < bottomBound - offset)
        {
            carPos.y = topBound + offset;
            transform.position = carPos;
        }
        else if (carPos.y > topBound + offset)
        {
            carPos.y = bottomBound - offset;
            transform.position = carPos;
        }
    }
}
