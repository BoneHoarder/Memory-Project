
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardsManager : MonoBehaviour
{
    [SerializeField] int columns= 4;
    [SerializeField] int rows= 4;

    [SerializeField] float gapX= 0.5f;
    [SerializeField] float gapY= 0.5f;

    [SerializeField] Sprite[] possibleFaces;

    [SerializeField] GameObject prefab;

    private int countWin=0;

    private List<CardBehaviour> cards= new();

    private List<CardBehaviour> faceUpCards= new();

    private int countCard {get{return rows * columns;}}

    private bool startChrono= false;
    private float chrono= 0f;



    // [SerializeField] Vector2 gameSize= Vector2.one * 4;
    
    void Start()
    {   
        // safeguards
        if(countCard %2 != 0){
            Debug.LogError("You need to have an even number of cards");
            return;
        }
        
        if(countCard / 2 > possibleFaces.Length){
                Debug.LogError($"You cannot have more cards than {possibleFaces.Length*2} cards.");
                return;
        }

        Initialize();
    }

    private void Initialize(){

        int nbFaces= countCard /2;

        List<int> faces=new();

        for(int n =0; n<nbFaces; n++){
            int face= Random.Range(0, possibleFaces.Length);

            while(faces.Contains(face)){
                face= Random.Range(0, possibleFaces.Length);
            }

            faces.Add(face);
        }

        faces.AddRange(faces);

        for(int x=0; x< columns; x++){
            for(int y=0; y< rows; y++){

                int index= Random.Range(0, faces.Count);

                InstantiateCard(x, y, faces[index]);
                faces.RemoveAt(index);
            }
        }

    }


    private void InstantiateCard(int x, int y, int faceID){

        GameObject card= Instantiate(prefab);

        if(card.TryGetComponent(out CardBehaviour cardBehaviour)){
            cards.Add(cardBehaviour);
            cardBehaviour.manager= this;
            GiveFace(cardBehaviour, faceID);

        }else{
            Debug.Log($"Prefab {prefab.name} does not have CardBehaviour");
        }

        Collider2D collider= card.GetComponent<Collider2D>();
        Vector3 cardSize= collider.bounds.size;

        float cardX = x * (cardSize.x+ gapX);

        float cardY = y * (cardSize.y + gapY);

        Vector3 position= new(cardX,cardY); 

        card.transform.SetParent(transform);
        card.transform.localPosition= position;

    }


    private void GiveFace(CardBehaviour cardBehaviour, int faceID){
        
        cardBehaviour.faceID= faceID;
        cardBehaviour.face= possibleFaces[faceID];

    }

[SerializeField] private UnityEvent whenPlayerWins;


    void Update()
    {   
        if(startChrono){
            chrono+= Time.deltaTime;
        }

        // Debug.Log($"Chrono: {chrono}");
    }

    public void CardHasBeenTurned(CardBehaviour card){
        faceUpCards.Add(card);
        if(faceUpCards.Count >1){
            if(faceUpCards[0].faceID != faceUpCards[1].faceID){
                faceUpCards[0].TurnBackAction();
                faceUpCards[1].TurnBackAction();

                // Debug.Log($"ID 1:{faceUpCards[0].faceID}, ID 2:{faceUpCards[1].faceID}");

            }
            else
            {
                countWin+=2;

                if(countWin== countCard){
                    Debug.Log($"You won!!");
                    startChrono= true; 


                    whenPlayerWins.Invoke();
                }
            }

            faceUpCards.Clear();
        }

        Debug.Log($"Points: {countWin}");
        
    }
}
