using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    internal class MatheusCard : Card {

        public MatheusCard(int cardId, int health, int attack, int mana) : base(cardId, health, attack, mana) { }

        public override void Battlecry() {
            base.Battlecry();
            Console.WriteLine("Teste de Battlecry");
        }

        public override Card CloneCard() {
            return new MatheusCard(CardId, Health, Attack, Mana);
        }
    }
}
