using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // All serializable variables.
    [Header("Dependencies")]
    [SerializeField] private GameObject fadeUI;
    [Header("Speed")]
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float distanceThreshold = 0.1f;
    [Header("Routes (A ROUTE MUST HAVE 4 NODES)")]
    [SerializeField] private Route[] routes;

    // Private Variables.
    private int routeTracker, pointCounter = 0;
    private GameObject target;
    private Rigidbody _rigidbody;
    private bool noRoutes, routeFinished, isFaded = false;
    private FadeScreen fade;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        fade = GetComponent<FadeScreen>();
    }

    private void Start()
    {
        target = routes[0].nodes[0];
    }

    private void Update()
    {
        MoveCamera();

        if (routeFinished)
            if (Input.GetKeyDown(KeyCode.Space))
                target = GetStartRoute();
    }

    /// <summary>
    /// Moves the Camera along the current Route.
    /// </summary>
    private void MoveCamera()
    {
        if (noRoutes || routeFinished)
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
                transform.position = targetPos;
            else if (pointCounter == 1)
            {
                if (!isFaded)
                    TriggerFade();

                // Move Camera by using rigidbody to start slow and end faster.
                _rigidbody.AddForce(movementSpeed * Vector3.Normalize(targetPos - transform.position));
            }
            else if (pointCounter == 3)
            {
                if(isFaded)
                    TriggerFade();

                // Move Camera by using a lerp to begin fast and end slow.
                // Make sure the lerp stop within the threshold to avoid it moving indefinetly.
                if (Vector3.Distance(transform.position, target.transform.position) > 0.5)
                    transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed * Time.deltaTime / 2);
                else
                {
                    // Declare route finished and increase route tracker.
                    print("Finished Route " + routeTracker);
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
            throw new Exception("NOTICE: ALL ROUTES FINISHED");
        }
        print("increased routeTracker to " + routeTracker);

    }

    private void FinalCutscene()
    {
        throw new NotImplementedException();
        //TODO: ADD BEHAVIOUR FOR FINAL CUTSCENE
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
            fade.ToggleFade();
            isFaded = !isFaded;
        }
    }
}

[System.Serializable]
public struct Route
{
    public GameObject[] nodes;
}
