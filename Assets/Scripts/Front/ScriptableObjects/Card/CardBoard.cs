using System;
using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using Assets.Scripts.Model;
using Assets.Scripts.ModsDTO;
using Assets.Scripts.Service;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.Faction
{
    public class CardBoard : UIBoard
    {
        private Card _card;

        public TextMeshProUGUI cardId;
        public TextMeshProUGUI cardTitle;
        public TextMeshProUGUI cardDescription;

        public Transform cardChoicesParent;
        public GameObject cardChoicesPrefab;
        public GameObject cardChoicePrefab;

        //public delegate void CarchoiceDelegate(CardChoice choice);
        //public CarchoiceDelegate callbacKFunc;
        public Action<CardChoice> callbackFunc;

        void Start()
        {
        }

        public override string key => "card";

        public void SetCard(Card card)
        {
            _card = card;
            BuildCard();
        }

        private void BuildCard()
        {
            cardId.text = string.Format(ModManager.Instance.GetSentence(SentenceDTO.CARD_ID),_card.id);
            cardTitle.text = _card.title;
            cardDescription.text = _card.description;

            var cardChoices = Instantiate(cardChoicesPrefab, cardChoicesParent);

            // gestion des choix
            foreach (CardChoice choice in _card.choices)
            {
                var cardChoice = Instantiate(cardChoicePrefab, cardChoices.transform);
                cardChoice.transform.Find("CardChoiceDescription").GetComponent<TextMeshProUGUI>().text = choice.label;
                CardChoiceLink cardChoiceLink = cardChoice.GetComponent<CardChoiceLink>();
                cardChoiceLink.callbacKFunc += CardChoiceCallBack;
                cardChoiceLink.choice = choice;

                // formatage des conséquences
                // première ligne : le navire
                // seconde ligne : l'île
                StringBuilder consequences = new StringBuilder();
                if (choice.shipId != null)
                {
                    choice.FormatedConsequence = string.Format(ModManager.Instance.GetSentence(SentenceDTO.CARD_CHOICE_SHIPBOARD),
                        ServiceGame.GetShip(choice.shipId).shipBoard.TextFromShipBoardDelta(choice.shipBoardDelta));
                    consequences.AppendLine(choice.FormatedConsequence);
                }

                cardChoice.transform.Find("CardChoiceConsequences").GetComponent<TextMeshProUGUI>().text = consequences.ToString();
            }
        }

        /// <summary>
        /// récupération du choix réalisé et application des conséquences
        /// </summary>
        /// <param name="choice"></param>
        private void CardChoiceCallBack(CardChoice choice)
        {
            callbackFunc(choice);
        }
    }
}
