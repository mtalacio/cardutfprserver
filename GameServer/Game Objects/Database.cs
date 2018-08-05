using System;
using System.Collections.Generic;
using GameServer.Card_Behaviours;
using GameServer.Utils;

namespace GameServer.Game_Objects {
    internal static class Database {
        private static readonly List<CardModel> ModelDatabase = new List<CardModel>();

        public static void InitializeCardDatabase() {
            Console.WriteLine("Initializing Card Database");
            ModelDatabase.Add(new MatheusCard(0, 10, 2, 1));
            ModelDatabase.Add(new TalacioCard(1, 2, 10, 2));
            Console.WriteLine("Card Database Initialized");
        }

        public static Card GetCard(int cardId, int serverId, int ownerIndex) {
            CardModel behaviourModel = ModelDatabase.Find(x => x.Id == cardId);
            
            if(behaviourModel == null)
                throw new BehaviourNotFoundException();

            Card toInstantiate = new Card(
                behaviourModel.Id, 
                behaviourModel.Health, 
                behaviourModel.Attack, 
                behaviourModel.Mana, 
                behaviourModel.GetBattlecry(), 
                behaviourModel.GetDeathrattle(),
                behaviourModel.PlayRequirements
            );

            toInstantiate.AssignCard(ownerIndex);

            return toInstantiate;
        }
    }
}
