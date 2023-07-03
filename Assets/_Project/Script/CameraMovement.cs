using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour
{
    // All serializable variables.
    [Header("Fading")]
    [SerializeField] private GameObject fadeUI;
    [SerializeField] private float secondsTilFadeIn = 1.0f;
    [Header("Speed")]
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float lerpMovementSpeed = 0.5f;
    [SerializeField] private float distanceThreshold = 0.1f;
    [Header("Dying timer")]
    [SerializeField] private float headacheDeathDelay;
    [Header("Audio")]
    [SerializeField] private AudioScriptPlayer audioScriptPlayer;
    [SerializeField] private int deathVoiceOverScene;
    [Header("Routes (A ROUTE MUST HAVE 4 NODES)")]
    [SerializeField] private Route[] routes;

    // Private Variables.
    private int routeTracker, pointCounter = 0;
    private GameObject target;
    private Rigidbody _rigidbody;
    private bool noRoutes, routeFinished, isFaded = false;
    private FadeScreen fade;
    private Animator _anim;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        fade = fadeUI.GetComponent<FadeScreen>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && target == null)
            target = routes[0].nodes[0];

        MoveCamera();

        if (routeFinished)
            if (Input.GetKeyDown(KeyCode.Space) && target != null)
                target = GetStartRoute();
    }

    public bool StartMoving()
    {
        if (target == null)
        {
            target = routes[0].nodes[0];
            return true;
        }
        else if (routeFinished)
        {
            target = GetStartRoute();
            return true;
        }
        return false;
        // return false, if player is not going to be progressing
    }

    /// <summary>
    /// Moves the Camera along the current Route.
    /// </summary>
    private void MoveCamera()
    {
        if (noRoutes || routeFinished || target == null)
            return;

        // If the camera is deemed close enough to the target position, 
        // Set rigidbody velocity to zero and get next target point
        if (Vector3.Distance(transform.position, target.transform.position) < distanceThreshold)
        {
            _rigidbody.velocity = Vector3.zero;
            if (!routeFinished)
                target = GetNextPoint();
        }

        print(pointCounter);

        if (target != null && !routeFinished)
        {
            Vector3 targetPos = target.transform.position;

            // If the pointCounter is an even number. Just teleport the camera to the exact location. 
            if (pointCounter % 2 == 0)
            {
                if (pointCounter == 0 && routes[routeTracker].invokeUponFirstNode != null)
                    routes[routeTracker].invokeUponFirstNode.Invoke();

                transform.position = targetPos;
            }
            else if (pointCounter == 1)
            {
                if (!isFaded)
                    StartCoroutine(FadeCountdown());

                // Move Camera by using rigidbody to start slow and end faster.
                _rigidbody.AddForce(movementSpeed * Vector3.Normalize(targetPos - transform.position));
            }
            else if (pointCounter == 3)
            {
                if (isFaded)
                    TriggerFade();

                // Move Camera by using a lerp to begin fast and end slow.
                // Make sure the lerp stop within the threshold to avoid it moving indefinetly.
                if (Vector3.Distance(transform.position, target.transform.position) > 0.5)
                    transform.position = Vector3.Lerp(transform.position, targetPos, lerpMovementSpeed * Time.deltaTime);
                else
                {
                    // Declare route finished and increase route tracker.
                    print("Finished Route " + routeTracker);
                    if (routes[routeTracker].invokeUponFinalNode != null)
                        routes[routeTracker].invokeUponFinalNode.Invoke();
                    IncreaseRouteTracker();
                    routeFinished = true;
                }
            }
        }
    }

    /// <summary>
    /// Increase the RouteTracker by 1 and reset the PointCounter to zero.
    /// Also declares noRoutes to be true if there are no more routes to take.
    /// </summary>
    private void IncreaseRouteTracker()
    {
        routeTracker++;
        pointCounter = 0;
        if (routes.Length - 1 < routeTracker)
        {
            noRoutes = true;
            FinalCutscene();
            print("NOTICE: ALL ROUTES FINISHED");
            return;
        }
        print("increased routeTracker to " + routeTracker);

    }

    private void FinalCutscene()
    {
        // Enable headache orbs (child position 1 and 2)
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(2).gameObject.GetComponent<Animator>().Play();
        StartCoroutine(StartFalling());
        //TODO: Snap to BLack
    }

    public void BlackscreenOfDeath()
    {
        fade.BlackScreenOfDeath();
    }

    private IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(headacheDeathDelay);
        _anim.enabled = true;
        yield return new WaitForSeconds(5);
        audioScriptPlayer.StartVoiceOverImmediate(deathVoiceOverScene);
        //_anim.SetTrigger("StartFalling");
    }

    /// <summary>
    /// Returns the first node in the current selected Route.
    /// </summary>
    /// <returns>GameObject</returns>
    private GameObject GetStartRoute()
    {
        routeFinished = false;
        print("Start New Route: Route " + routeTracker);
        return routes[routeTracker].nodes[0];
    }

    /// <summary>
    /// Returns the next node in the current Route.
    /// Increases the route if the next node is outside of the scope of the Route
    /// </summary>
    /// <returns></returns>
    private GameObject GetNextPoint()
    {
        if (routes.Length == 0 || routes.Length - 1 < routeTracker)
        {
            noRoutes = true;
            throw new Exception("NOTICE: NO ROUTES FOUND");
        }

        pointCounter++;

        // Make sure the new point doesn't cause a IndexOutOfRangeException. 
        // If it does, Increase RouteTracker and declare Route finished
        if (pointCounter > routes[routeTracker].nodes.Length - 1)
        {
            IncreaseRouteTracker();
            routeFinished = true;
        }

        return routes[routeTracker].nodes[pointCounter];
    }

    /// <summary>
    /// Triggers the FadeScreen to fade out or in dependend on the state.
    /// </summary>
    private void TriggerFade()
    {
        if (fade != null)
        {
            print("Triggered Fade");
            fade.ToggleFade();
            isFaded = !isFaded;
        }
    }

    private IEnumerator FadeCountdown()
    {
        isFaded = true;
        yield return new WaitForSeconds(secondsTilFadeIn);
        fade.ToggleFade();
    }
}

[System.Serializable]
public struct Route
{
    public GameObject[] nodes;
    public UnityEvent invokeUponFirstNode;
    public UnityEvent invokeUponFinalNode;
}
