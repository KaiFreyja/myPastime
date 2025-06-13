using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleMainUnitCase : MonoBehaviour
{
    BattleMain main = null;

    Master[] players = null;
    Master[] enemys = null;
    Card[] cards = new Card[5];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main = GetComponent<BattleMain>();
        main.onChangeBattleState = (BattleMain.BattleState pre, BattleMain.BattleState now) =>
        {
            switch (now)
            {
                case BattleMain.BattleState.INTO_ROUND:
                    players = main.GetCurrentPlayerMaster();
                    enemys = main.GetCurrentEnemyMaster();
                    break;

                case BattleMain.BattleState.PLAYER_ACTION_WAIT:
                    isSelectCard = false;
                    selectCards = new List<Card>();
                    cards = main.GetNowRoundCard();
                    break;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        switch (main.state)
        {
            case BattleMain.BattleState.WAIT:
                uiWait();
                break;

            case BattleMain.BattleState.PLAYER_ACTION_WAIT:
                uiShowMissionData();
                uiShowMaster();
                if (isSelectCard)
                {
                    uiPlayerActionWaitSelectCard();
                }
                else
                {
                    uiPlayerActionWait();
                }
                break;

            case BattleMain.BattleState.RESULT:
                uiResult();
                break;
        }
    }

    private void uiWait()
    {
        if (GUI.Button(new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200), "開始"))
        {
            main.GameStart();
        }
    }
    bool isSelectCard = false;
    List<Card> selectCards = new List<Card>();
    private void uiPlayerActionWaitSelectCard()
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

        int w = 150;
        int h = 150;
        for (int i = 0; i < cards.Length; i++)
        {
            Card card = cards[i];
            bool isSelect = selectCards.Contains(card);
            Master master = main.GetCardToMaster(card);
            string text = master.name + "\n" + (isSelect ? "[" + card.color.ToString() + "]" + "\n" + selectCards.IndexOf(card) : card.color.ToString());
            if (GUI.Button(new Rect(Screen.width / (cards.Length + 1) * (i + 1) - w / 2, (Screen.height - h) / 2, w, h), text))
            {
                if (isSelect)
                {
                    selectCards.Remove(card);
                }
                else
                {
                    selectCards.Add(card);
                    if (selectCards.Count == 3)
                    {
                        main.PlayerCards(selectCards.ToArray(), enemys[selectEnemy]);
                        isSelectCard = false;
                    }
                }
            }
        }

        w = 100;
        h = 100;
        if (GUI.Button(new Rect(Screen.width - w, Screen.height - h, w, h), "取消"))
        {
            isSelectCard = false;
        }
    }

    private void uiPlayerActionWait()
    {
        int w = 100;
        int h = 100;

        if (GUI.Button(new Rect(Screen.width - w, Screen.height - h, w, h), "出牌"))
        {
            isSelectCard = true;
        }
    }

    int selectEnemy = 0;

    private void uiShowMaster()
    {
        int w = 100;
        int h = 200;

        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];
            GUI.Box(new Rect(w * (i + 1) + 20 * i, Screen.height - h, w, h), player.name + "\n" + player.hp + "/" + player.maxHp);
        }

        for (int i = 0; i < enemys.Length; i++)
        {
            var enemy = enemys[i];
            string name = (selectEnemy == i) ? "[" + enemy.name + "]" : enemy.name;
            if (GUI.Button(new Rect(w * (i + 1) + 20 * i, 0, w, h), name + "\n" + enemy.hp + "/" + enemy.maxHp))
            {
                selectEnemy = i;
            }
        }
    }
    private void uiShowMissionData()
    {
        int w = 100;
        int h = 100;

        int max = main.GetMaxMissionsNum();
        int now = main.GetNowMissionsNum();
        int nowRound = main.GetRoundNum();

        GUI.Box(new Rect(Screen.width - w, 0, w, h), "[" + now + "/" + max + "]" + "\n" + "Round : " + nowRound);
        
    }

    private void uiResult()
    {
        bool isWin = main.GetIsWin();
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), isWin?"Win":"Lose");

        int w = 100;
        int h = 100;
        if (GUI.Button(new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h), "重新開始"))
        {
            main.GameStart();
        }
    }
}
