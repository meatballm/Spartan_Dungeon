using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit[] hits = Physics.RaycastAll(ray, maxCheckDistance+ PlayerController.Instance.camDistance, layerMask);
            float closestDist = float.MaxValue;
            GameObject bestObj = null;
            IInteractable bestInteract = null;

            foreach (var h in hits)
            {
                if (h.distance <= PlayerController.Instance.camDistance)
                    continue;
                if (h.collider.gameObject == this.gameObject)
                    continue;
                if (h.distance < closestDist)
                {
                    closestDist = h.distance;
                    bestObj = h.collider.gameObject;
                    bestInteract = h.collider.GetComponent<IInteractable>();
                }
            }
            if (bestObj != null)
            {
                if (bestObj != curInteractGameObject)
                {
                    curInteractGameObject = bestObj;
                    curInteractable = bestInteract;
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}