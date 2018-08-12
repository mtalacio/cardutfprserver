using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    internal class TalacioCard : CardModel {
        public TalacioCard(int id, int health, int attack, int mana, bool isTaunt) : base(id, health, attack, mana, isTaunt) {
            PlayRequirements = new Dictionary<Enums.PlayRequirement, bool> {
                {Enums.PlayRequirement.MINIONS_ON_BOARD, false},
                {Enums.PlayRequirement.ENEMIES_ON_BOARD, false}
            };
        }
    }
}
