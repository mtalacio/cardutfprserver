using System.Collections.Generic;

namespace GameServer.Card_Behaviours {
    public class HeroKillerCard : CardModel {

        public HeroKillerCard(int id, int health, int attack, int mana, bool isTaunt) : base(id, health, attack, mana, isTaunt) {
            PlayRequirements = new Dictionary<Enums.PlayRequirement, bool> {
                {Enums.PlayRequirement.MINIONS_ON_BOARD, false},
                {Enums.PlayRequirement.ENEMIES_ON_BOARD, false}
             };
        }
    }
}
