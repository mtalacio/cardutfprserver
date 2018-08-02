using static GameServer.Enums;

namespace GameServer.Game_Objects {
    internal class Card {
        public int CardId { get; }
        public CardPlace Place { get; private set; }
        public int OwnerIndex { get; }
        

        public Card(int cardId, int ownerIndex) {
            CardId = cardId;
            Place = CardPlace.HAND;
            OwnerIndex = ownerIndex;
        }

        public void ChangePlace(CardPlace place) {
            Place = place;
        }
    }
}
