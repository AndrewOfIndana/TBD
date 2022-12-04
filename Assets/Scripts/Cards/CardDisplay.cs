using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public int x;
    [HideInInspector] public int y;

    private Card _card;
    public GameObject hiddenUI;
    public CardInfo hiddenCardInfo;

public Card Card
{
        // gives the card on the screen images
    get => _card;
    set
    {
        if (_card == value) return;
        _card = value;
            icon.sprite = _card.cardImage;
    }
}
    public Image icon;
    public Button button;
    //sets card location on the screen
    public CardDisplay Left => x > 0 ? DeckCreator.Instance.Cards[x - 1, y] : null;
    public CardDisplay Right => x > 0 ? DeckCreator.Instance.Cards[y, x + 1] : null;

    public CardDisplay[] Neighbors => new[]
    {
        Left,Right
    };
    private void Start() => button.onClick.AddListener(() => DeckCreator.Instance.Select(this));

   
    public List<CardDisplay> CardsInRow(List<CardDisplay> exclude = null)
    {
        var result = new List<CardDisplay> { this, };
        if (exclude == null)
        {
            exclude = new List<CardDisplay> { this, };
        }
        else { exclude.Add(this);
        }
        foreach(var neighbour in Neighbors)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Card != Card) continue;
            result.AddRange(neighbour.CardsInRow(exclude));
        }
        return result;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hiddenUI.SetActive(true);
        hiddenCardInfo.SetCardInfo(_card);
    }
    public void HideEffect()
    {
        hiddenUI.SetActive(false);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        hiddenUI.SetActive(false);
    }
}

