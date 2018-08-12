using System;
using System.Collections.Generic;
using GameServer.Card_Behaviours;
using GameServer.Utils;

namespace GameServer.Game_Objects {
    internal static class Database {
        private static readonly List<CardModel> ModelDatabase = new List<CardModel>();

        public static void InitializeCardDatabase() {
            Console.WriteLine("Initializing Card Database");
            ModelDatabase.Add(new Hero(0, 20, 0, 0, false));
            ModelDatabase.Add(new TalacioCard(1, 5, 1, 2, true));
            ModelDatabase.Add(new CoinCard(2, 0));
            ModelDatabase.Add(new MatheusCard(3, 10, 2, 1, false));
            ModelDatabase.Add(new HeroKillerCard(4, 30, 15, 0, false));
            Console.WriteLine("Card Database Initialized");
        }

        public static Card GetCard(int cardId, int ownerIndex) {
            CardModel behaviourModel = ModelDatabase.Find(x => x.Id == cardId);
            
            if(behaviourModel == null)
                throw new BehaviourNotFoundException();

            Card toInstantiate;
            if (!behaviourModel.IsSpell)
                toInstantiate = new Card(
                    behaviourModel.Id,
                    behaviourModel.Health,
                    behaviourModel.Attack,
                    behaviourModel.Mana,
                    behaviourModel.IsTaunt,
                    behaviourModel.GetBattlecry(),
                    behaviourModel.GetDeathrattle(),
                    behaviourModel.PlayRequirements
                );
            else
                toInstantiate = new Card(
                    behaviourModel.Id,
                    behaviourModel.Mana,
                    behaviourModel.GetBattlecry(),
                    behaviourModel.PlayRequirements
                );

            toInstantiate.AssignCard(ownerIndex);

            return toInstantiate;
        }
    }
}
