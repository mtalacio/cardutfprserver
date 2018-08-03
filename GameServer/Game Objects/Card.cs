using System;
using System.Collections.Generic;
using static GameServer.Enums;

namespace GameServer.Game_Objects {

    internal delegate void Battlecry(Card cardBase);
    internal delegate void Deathrattle(Card cardBase);
    internal delegate bool CanBePlayed(Card cardBase);

    internal class Card {

        public int ServerId { get; private set; }
        public CardPlace Place { get; private set; }
        public int OwnerIndex { get; private set; }

        public int CurrentHealth { get; private set; }
        public int CurrentAttack { get; private set; }
        public int CurrentMana { get; private set; }

        public int CardId { get; }

        public readonly int Health;
        public readonly int Attack;
        public readonly int Mana;

        private readonly Battlecry _battlecries;
        private readonly Deathrattle _deathrattles;
        private readonly List<CanBePlayed> _canBePlayeds = new List<CanBePlayed>();

        public bool JustPlayed { get; private set; }

        public Card(int cardId, int health, int attack, int mana, Battlecry baseBattlecry, Deathrattle baseDeathrattle, CanBePlayed canPlayChecks) {
            Place = CardPlace.DECK;
            CardId = cardId;

            Health = health;
            Attack = attack;
            Mana = mana;

            CurrentHealth = health;
            CurrentAttack = attack;
            CurrentMana = mana;

            if(baseBattlecry != null)
                _battlecries += baseBattlecry;

            if(baseDeathrattle != null)
                _deathrattles += baseDeathrattle;

            _canBePlayeds.Add(canPlayChecks);
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

        public bool CanPlay() {
            foreach (CanBePlayed canPlay in _canBePlayeds) {
                bool result = canPlay.Invoke(this);
                if (!result)
                    return false;
            }

            return true;
        }

        public bool CanAttack() {
            return !JustPlayed;
        }

        public void WakeUp() {
            JustPlayed = false;
        }

        public void PlayCard() {
            ChangePlace(CardPlace.BOARD);
            JustPlayed = true;
            CallBattlecries();
        }

        private void CallBattlecries() {
            _battlecries?.Invoke(this);
        }

        private void CallDeathrattles() {
            _deathrattles?.Invoke(this);
        }

    }
}
