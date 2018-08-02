using System;
using System.Collections.Generic;
using GameServer.Card_Behaviours;
using GameServer.Utils;

namespace GameServer.Game_Objects {
    internal static class Database {
        private static readonly List<Card> CardDatabase = new List<Card>();

        public static void InitializeCardDatabase() {
            Console.WriteLine("Initializing Card Database");
            CardDatabase.Add(new MatheusCard(0, 10, 2, 1));
            CardDatabase.Add(new TalacioCard(1, 2, 10, 2));
            Console.WriteLine("Card Database Initialized");
        }

        public static Card GetCard(int cardId, int serverId, int ownerIndex) {
            Card toInstantiate = CardDatabase.Find(x => x.CardId == cardId).CloneCard();
            
            if(toInstantiate == null)
                throw new CardNotFoundException();

            toInstantiate.InstantiateCard(serverId, ownerIndex);

            return toInstantiate;
        }
    }
}
