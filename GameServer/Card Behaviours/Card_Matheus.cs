using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    internal class MatheusCard : CardModel {

        public MatheusCard(int id, int health, int attack, int mana) : base(id, health, attack, mana) { }

        public override Battlecry GetBattlecry() {
            return model => Console.WriteLine("Teste Battlecry");
        }
    }
}
