using System.Collections.Generic;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    public class CardModel {
        public int Id { get; }

        public int Health { get; }
        public int Attack { get; }
        public int Mana { get; }

        public bool IsTaunt { get; }

        public bool IsSpell { get; }

        public Dictionary<Enums.PlayRequirement, bool> PlayRequirements { get; protected set; }

        protected CardModel(int id, int health, int attack, int mana, bool isTaunt) {
            Id = id;
            Health = health;
            Attack = attack;
            Mana = mana;
            IsTaunt = isTaunt;
            IsSpell = false;
        }

        protected CardModel(int id, int mana) {
            Id = id;
            Mana = mana;
            IsSpell = true;
        }

        public virtual Battlecry GetBattlecry() {
            return null;
        }

        public virtual Deathrattle GetDeathrattle() {
            return null;
        }
    }
}
