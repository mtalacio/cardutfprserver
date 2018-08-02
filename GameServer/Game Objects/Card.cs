using System;
using GameServer.Card_Behaviours;
using static GameServer.Enums;

namespace GameServer.Game_Objects {
    internal class Card {

        public int ServerId { get; private set; }
        public CardPlace Place { get; private set; }
        public int OwnerIndex { get; private set; }

       

        public int CurrentHealth { get; private set; }
        public int CurrentAttack { get; private set; }
        public int CurrentMana { get; private set; }

        public int CardId { get; }

        protected int Health;
        protected int Attack;
        protected int Mana;

        public Card(int cardId, int health, int attack, int mana) {
            Place = CardPlace.HAND;
            CardId = cardId;

            Health = health;
            Attack = attack;
            Mana = mana;

            CurrentHealth = health;
            CurrentAttack = attack;
            CurrentMana = mana;
        }

        public virtual Card CloneCard() {
            return null;
        }

        public void InstantiateCard(int serverId, int ownerIndex) {
            ServerId = serverId;
            OwnerIndex = ownerIndex;
        }

        public void ChangePlace(CardPlace place) {
            Console.WriteLine("Changing card SID " + ServerId + " from: " + Place + " to: " + place);
            Place = place;
        }

        public void ChangeHealth(int newHealth) {
            CurrentHealth = newHealth;
        }

        public void ChangeAttack(int newAttack) {
            CurrentAttack = newAttack;
        }

        public void ChangeMana(int newMana) {
            CurrentMana = newMana;
        }

        public void PlayCard() {
            ChangePlace(CardPlace.BOARD);
            Battlecry();
        }

        public virtual void Battlecry() {

        }
    }
}
