using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public Transform interactbaleTransform;
    public SpriteRenderer spriteRenderer;
    public string correspondingScriptFileName;
    public bool reclickable;

    [Tooltip("Unique ID used to remember one-time interactables across scenes. Leave empty to use SceneName/ObjectName.")]
    public string interactableID;

    void Awake()
    {
        if (string.IsNullOrEmpty(interactableID))
        {
            interactableID = gameObject.scene.name + "/" + gameObject.name;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactbaleTransform = this.transform;
    }

    private void OnMouseDown()
    {
        GameManager manager = GameManager.Instance;
        if (manager == null || manager.dialogueHandler == null)
        {
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!manager.dialogueHandler.scriptFinished)
        {
            return;
        }

        if (!reclickable)
        {
            if (manager.IsInteractableUsed(interactableID))
            {
                return;
            }
            manager.MarkInteractableUsed(interactableID);
        }

        manager.changeScript(correspondingScriptFileName);
    }

    public void changeScript(string IDstring)
    {
        correspondingScriptFileName = IDstring;
    }

    public void deactivate()
    {
        this.gameObject.SetActive(false);
    }

}