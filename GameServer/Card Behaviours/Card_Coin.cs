using System.Collections.Generic;
using GameServer.Game_Objects;

namespace GameServer.Card_Behaviours {
    public class CoinCard : CardModel {

        public CoinCard(int id, int mana) : base(id, mana) {
            PlayRequirements = new Dictionary<Enums.PlayRequirement, bool> {
                {Enums.PlayRequirement.MINIONS_ON_BOARD, false},
                {Enums.PlayRequirement.ENEMIES_ON_BOARD, false}
            };
        }

        public override Battlecry GetBattlecry() {
            return card => {
                GameEngine.MathAvailableManaTo(card.OwnerIndex, 1);
                GameEngine.DestroyCardTo(card.OwnerIndex, card.ServerId);
                GameEngine.AddToGraveyard(card);
            };
        }
    }
}
