using System;
using System.Collections.Generic;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    public class FiremanCard : CardModel {
        public FiremanCard(int id, int health, int attack, int mana, bool isTaunt) : base(id, health, attack, mana,
            isTaunt) {

            PlayRequirements = new Dictionary<Enums.PlayRequirement, bool> {
                {Enums.PlayRequirement.MINIONS_ON_BOARD, false},
                {Enums.PlayRequirement.ENEMIES_ON_BOARD, true}
            };

        }

        public override Battlecry GetBattlecry() {
            return card => {
                GameEngine.OnCardSelectedCallback += ResolveBattlecry;
                Dictionary<Enums.TargetRequirement, int> TargetRequirements = new Dictionary<Enums.TargetRequirement, int> {
                    {Enums.TargetRequirement.ENEMY_MINION, 0}
                };
                GameEngine.PromptTargets(TargetRequirements);
            };
        }

        private void ResolveBattlecry(long sId) {
            GameEngine.DamageCard((int)sId, 2);
            GameEngine.EndTargetEvent(ResolveBattlecry);
        }
    }
}
