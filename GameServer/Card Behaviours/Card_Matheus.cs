using System;
using System.Collections.Generic;
using GameServer.Game_Objects;
using static GameServer.Enums;

namespace GameServer.Card_Behaviours {
    internal class MatheusCard : CardModel {

        public MatheusCard(int id, int health, int attack, int mana) : base(id, health, attack, mana) {
            PlayRequirements = new Dictionary<Enums.PlayRequirement, bool> {
                {PlayRequirement.MINIONS_ON_BOARD, false},
                {PlayRequirement.ENEMIES_ON_BOARD, false}
            };
        }

        public override Battlecry GetBattlecry() {
            return model => Console.WriteLine("Teste Battlecry");
        }
    }
}
