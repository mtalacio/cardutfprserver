using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameServer.Enums;

namespace GameServer.Game_Objects {
    class Card {
        public int CardId { get; }
        public CardPlace Place { get; set; }
        public int OwnerIndex { get; }
        

        public Card(int cardId, int ownerIndex) {
            CardId = cardId;
            Place = CardPlace.HAND;
            OwnerIndex = ownerIndex;
        }

    }
}
