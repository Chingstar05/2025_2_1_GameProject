using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("상호작용 정보")]
    public string objectName = "아이템";
    public string interactionText = "[E] 상호작용";
    public InteractionType interactionType = InteractionType.item;

    [Header("하이라이트 설정")]
    public Color highlightColor = Color.yellow;
    public float highlightlntensity = 1.5f;

    public Renderer ObjectRenderer;
    private Color orignialColor;
    private bool isHighlighted = false;

    public enum InteractionType
    {
        item,
        Machine,
        Builing,
        Npc,
        Collectible
    }

    // Start is called before the first frame update
    
    // Update is called once per frame

    protected virtual void Start()
    {
        ObjectRenderer = GetComponent<Renderer>();
        if(ObjectRenderer != null)
        {
            orignialColor = ObjectRenderer.material.color;
        }

        gameObject.layer = 8;
    }

    public virtual void OnPlayerEnter()
    {
        Debug.Log($"{objectName})감지됨");
        HighlightObject();
    }
    public virtual void OnPlayerExit()
    {
        Debug.Log($"[{objectName}]범위에서 벗어남");
        RemoveHighlight();
    }
    
    public virtual void Interact()
    {
        //상호작용 타입에 따른 기본 동작
        switch (interactionType)
        {
            case InteractionType.item:
                Collectitem();
                break;
            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Builing:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                Collectitem();
                break;
        }
    }

    public string GetInteractionText()
    {
        return interactionText;
    }



    protected virtual void HighlightObject()
    {
        if (ObjectRenderer != null && !isHighlighted)
        {
            ObjectRenderer.material.color = highlightColor;
            ObjectRenderer.material.SetFloat("_Emission", highlightlntensity);
            isHighlighted = true;
        }


    }
    protected virtual void RemoveHighlight()
    {
        if (ObjectRenderer != null && isHighlighted)
        {
            ObjectRenderer.material.color = orignialColor;
            ObjectRenderer.material.SetFloat("_Emission", 0f);
            isHighlighted = false;
        }
    }
    protected virtual void Collectitem()
    {
        Debug.Log($"{objectName}을(를) 획득했습니다");
        Destroy(gameObject);
    }
    protected virtual void OperateMachine()
    {
        Debug.Log($"{objectName}을(를) 작동시켰습니다");
        if (ObjectRenderer != null)
        {
            ObjectRenderer.material.color = Color.green;
        }

    }
    protected virtual void AccessBuilding()
    {
        Debug.Log($"{objectName}을(를) 에 접근했습니다");
        transform.Rotate(Vector3.up * 90f);
    }
    protected virtual void TalkToNPC()
    {
        Debug.Log($"{objectName}와 대화를 시작합니다");
    }
}
