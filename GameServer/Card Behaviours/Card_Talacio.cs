using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    internal class TalacioCard : Card {
        public TalacioCard(int cardId, int health, int attack, int mana) : base(cardId, health, attack, mana) { }

        public override Card CloneCard() {
            return new TalacioCard(CardId, Health, Attack, Mana);
        }
    }
}
