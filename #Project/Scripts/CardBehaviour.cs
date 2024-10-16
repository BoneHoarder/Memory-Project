using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class CardBehaviour : MonoBehaviour
{
    public CardsManager manager;

    [SerializeField] internal Sprite face;

    internal int faceID;

    private Sprite back;

    private SpriteRenderer renderS;

    private bool faceUp= false;

    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        renderS= GetComponent<SpriteRenderer>();
        back = renderS.sprite;

        animator= GetComponent<Animator>();
    }

    public void Turn(){
        if(faceUp){
            renderS.sprite= back;

        }else{
            renderS.sprite= face;
        }

        faceUp= !faceUp;
    }

    public void TurnAction(){
        animator.SetTrigger("turn");
        animator.SetBool("hover", false);

        manager.CardHasBeenTurned(this);
    }

    public void TurnBackAction(){
        animator.SetTrigger("turn back");
    }

    void OnMouseDown(){
        if(!faceUp){
            TurnAction();
        }
    }

    void OnMouseEnter(){
        if(!faceUp){
            animator.SetBool("hover", true);
        }
    }

    void OnMouseExit(){
        if(!faceUp){
            animator.SetBool("hover", false);
        }
    }
}
