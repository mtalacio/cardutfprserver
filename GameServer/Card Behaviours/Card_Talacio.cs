using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    internal class TalacioCard : CardModel {
        public TalacioCard(int id, int health, int attack, int mana) : base(id, health, attack, mana) { }
    }
}
