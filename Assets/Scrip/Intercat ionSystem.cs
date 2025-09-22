using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntercationSystem : MonoBehaviour
{

    [Header("상호 작용 설정")]
    public float interactionRage = 2.0f;
    public LayerMask interactionLayerMask = 1;
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI 설정")]
    public Text interactionText;                            //상호작용 UI텍스트
    public GameObject interactionUI;                        //상호작용 UI 패널

    private Transform playerTransform;
    private InteractableObject currentlnteractiable;   //감지된 오브젝트 담는 클래스

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        HidelnteractionUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
        HandlelnteractionInput();
    }

    void CheckForInteractables()
    {
        Vector3 checkPosition = playerTransform.position + playerTransform.forward * (interactionRage * 0.5f);

        Collider[] hitColliders = Physics.OverlapSphere(checkPosition,interactionRage, interactionLayerMask);

        InteractableObject closestInteractable = null;
        float closestDistance = float.MaxValue;

        //가장 가까운 상호작용 오브젝트 찾기
        foreach (Collider collider in hitColliders)
        {
            InteractableObject interactable = collider.GetComponent<InteractableObject>();
            if(interactable != null)
            {
                float distance = Vector3.Distance(playerTransform.position, collider.transform.position);

                Vector3 directionToObject = (collider.transform.position - playerTransform.position).normalized;
                float angle = Vector3.Angle(playerTransform.forward,directionToObject);


                if (angle < 90f && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }
        if(closestInteractable != null)
        {
            if(currentlnteractiable != null)
            {
                currentlnteractiable.OnPlayerExit();
            }

            currentlnteractiable = closestInteractable;

            if(currentlnteractiable != null)
            {
                currentlnteractiable.OnPlayerEnter();
                ShowInteractionUI(currentlnteractiable.GetInteractionText());
            }
            else
            {
                HidelnteractionUI();
            }
        }
    }
    


    void HandlelnteractionInput()
    {
        if(currentlnteractiable != null && Input.GetKeyUp(interactionKey))
        {
            currentlnteractiable.Interact();
        }
    }

    void ShowInteractionUI(string text)
    {
        if(interactionUI != null)
        {
            interactionUI.SetActive(true);
        }

        if(interactionText != null)
        {
            interactionText.text = text; 
        }


    }


    void HidelnteractionUI()
    {
        if(interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }
}
