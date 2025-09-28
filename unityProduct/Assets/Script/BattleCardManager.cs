using System.Collections.Generic;
using UnityEngine;

public interface BattleCardProvider
{
    public Master[] GetCurrentPlayerTeam();
}


public class BattleCardManager
{
    public BattleCardProvider cardProvider = null;

    /// <summary>
    /// 目前牌組
    /// </summary>
    public Queue<Card> playersCards = new Queue<Card>();
    /// <summary>
    /// 該回合的卡牌
    /// </summary>
    public List<Card> currentsRoundCards = new List<Card>();

    /// <summary>
    /// 發牌
    /// </summary>
    public void dealCards()
    {
        if (playersCards.Count < 5)
        {
            initPlayerCard();
        }

        currentsRoundCards = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            currentsRoundCards.Add(playersCards.Dequeue());
        }
    }

    /// <summary>
    /// 初始化牌組順序
    /// </summary>
    public void initPlayerCard()
    {
        playersCards = new Queue<Card>();
        var currentPlayerTeam = cardProvider.GetCurrentPlayerTeam();
        //初始牌型
        var cards = new List<Card>();
        for (int i = 0; i < currentPlayerTeam.Length; i++)
        {
            Master master = currentPlayerTeam[i];
            if (master == null)
            {
                continue;
            }
            for (int j = 0; j < master.cards.Count; j++)
            {
                cards.Add(master.cards[j]);
            }
        }
        //洗牌
        for (int i = 0; i < cards.Count; i++)
        {
            int random = UnityEngine.Random.Range(0, cards.Count - 1);
            var temp = cards[i];
            cards[i] = cards[random];
            cards[random] = temp;
        }

        foreach (var a in cards)
        {
            playersCards.Enqueue(a);
        }
    }
}
