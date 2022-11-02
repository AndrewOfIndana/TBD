using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class CardDisplay : MonoBehaviour
{
    public int x;
    public int y;

    private Card _card;


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
    public CardDisplay Top => y > 0 ? DeckCreator.Instance.Cards[x, y - 1] : null;
    public CardDisplay Bottom => x < 0 ? DeckCreator.Instance.Cards[x, y + 1] : null;

    private void Start() => button.onClick.AddListener(() => DeckCreator.Instance.Select(this));













}

