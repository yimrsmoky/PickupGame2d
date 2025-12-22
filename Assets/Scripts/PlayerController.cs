using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    //private Vector2 moveDirection = Vector2.up;
    private Vector2 startTouchPosition; //стартовая позиция касания
    private Vector2 endTouchPosition; //конечная позиция касания
    //private bool isDragging = false; //палец на экране?
    private float swipeThreshold = 50f; //чувствительность свайпа

    private Camera cam;
    private float screenHeight;
    private float screenWidth;
    public float offset = 0.1f;
    private float leftBound, rightBound, topBound, bottomBound;

    private AudioSource carAudioSource;
    public AudioClip normalMotorSound;
    public AudioClip boostedMotorSound;

    [SerializeField] private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float boostedSpeed;

    public Quaternion targetRotation;
    private Quaternion swapRotation;

    public bool isBoosted;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;

        cam = Camera.main;
        screenHeight = 2f * cam.orthographicSize;
        screenWidth = screenHeight * cam.aspect;

        leftBound = cam.transform.position.x - screenWidth / 2;
        rightBound = cam.transform.position.x + screenWidth / 2;
        bottomBound = cam.transform.position.y - screenHeight / 2;
        topBound = cam.transform.position.y + screenHeight / 2;
        carAudioSource = GetComponent<AudioSource>();

        targetRotation = transform.rotation;
    }
    void Update()
    {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject()) return;
            PCInput();

#elif UNITY_ANDROID
        if (EventSystem.current.IsPointerOverGameObject()) return;
        MobileInput();
#endif

        TeleportToOpposideSide();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.isStarted)
        {
            MoveCarForward();
            RotateCar(targetRotation);
        }
    }
    void MobileInput()
    {
        if (Input.touchCount > 0) // цкъа хьокхавалар мукъаг1 дале
        {
            Touch touch = Input.GetTouch(0); // хьалхарча хьокхаваларе меттиг хьаэц вай

            if (touch.phase == TouchPhase.Began) // хьокхавалар д1адоладена дале
            {
                startTouchPosition = touch.position;
                //isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended) // хьокхавалар чакхадаьннадале
            {
                endTouchPosition = touch.position;
                //isDragging = false;
                ProcessSwipe();
            }
        }
    }
    void PCInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            //isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            //isDragging = false;
            ProcessSwipe();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) RotateCar(Quaternion.Euler(0, 0, 0));
        if (Input.GetKeyDown(KeyCode.DownArrow)) RotateCar(Quaternion.Euler(0, 0, 180));
        if (Input.GetKeyDown(KeyCode.RightArrow)) RotateCar(Quaternion.Euler(0, 0, -90));
        if (Input.GetKeyDown(KeyCode.LeftArrow)) RotateCar(Quaternion.Euler(0, 0, 90));

    }
    void ProcessSwipe()
    {

        Vector2 swipeDelta = endTouchPosition - startTouchPosition; //хокхавалар чакхдалара е из доладалара е юкъара юкъ
        bool isPaused = GameManager.Instance.isPaused;
        bool isStarted = GameManager.Instance.isStarted;
        if (isPaused || !isStarted) return;

        //свайпи йоахал мишт я хьожа вай
        if (swipeDelta.magnitude >= swipeThreshold)
        {
            Vector2 direction = swipeDelta.normalized; //йоахал 1 оттаю вай (нормализовать ю)

            //свайпо де дезар ухаза да
            //CarMovement
            bool IsHorizontalSwipe = Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y);

            if (IsHorizontalSwipe)
            {
                if (swipeDelta.x > 0)
                {
                    swapRotation = Quaternion.Euler(0, 0, -90);
                }
                if (swipeDelta.x < 0)
                {
                    swapRotation = Quaternion.Euler(0, 0, 90);
                }
            }
            else
            {
                if (swipeDelta.y > 0)
                {
                    swapRotation = Quaternion.Euler(0, 0, 0);
                }
                if (swipeDelta.y < 0)
                {
                    swapRotation = Quaternion.Euler(0, 0, 180);
                }
            }

            RotateCar(swapRotation);

        }
        else
        {
            //клик
            Tap();
        }
    }
    void MoveCarForward()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);

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
    void RotateCar(Quaternion inputRotation)
    {
        if (Quaternion.Angle(inputRotation, targetRotation) != 180f)
        {
            targetRotation = inputRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
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
    public void Tap()
    {
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
