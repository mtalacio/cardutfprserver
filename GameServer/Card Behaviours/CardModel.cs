using System.Collections.Generic;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    internal class CardModel {
        public int Id { get; }

        public int Health { get; }
        public int Attack { get; }
        public int Mana { get; }

        public Dictionary<Enums.PlayRequirement, bool> PlayRequirements { get; protected set; }

        protected CardModel(int id, int health, int attack, int mana) {
            Id = id;

            Health = health;
            Attack = attack;
            Mana = mana;
        }

        public virtual Battlecry GetBattlecry() {
            return null;
        }

        public virtual Deathrattle GetDeathrattle() {
            return null;
        }
    }
}
