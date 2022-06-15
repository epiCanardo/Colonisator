using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using Assets.Scripts.Model;
using Assets.Scripts.ModsDTO;
using TMPro;
using UnityEngine;

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

        void Start()
        {
            BuildCard();
        }

        public override string key => "card";

        public void SetCard(Card card)
        {
            _card = card;
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
                cardChoice.transform.Find("CardChoiceConsequences").GetComponent<TextMeshProUGUI>().text = choice.shipBoardDelta.ToString();
            }
        }
    }
}
