using System;
using System.Collections.Generic;
using GameServer.Utils;
using static GameServer.Enums;

namespace GameServer.Game_Objects {

    internal delegate void Battlecry(Card cardBase);
    internal delegate void Deathrattle(Card cardBase);
    internal delegate bool CanBePlayed(Card cardBase);

    internal class Card {

        public static int LastServerId { get; }

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
        private readonly Dictionary<PlayRequirement, bool> _playReqs;

        public bool JustPlayed { get; private set; }

        public Card(int cardId, int health, int attack, int mana, Battlecry baseBattlecry, Deathrattle baseDeathrattle, Dictionary<PlayRequirement, bool> playReqs) {
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

            _playReqs = playReqs;
        }

        public Card CloneCard() {
            return new Card(CardId, CurrentHealth, CurrentAttack, CurrentMana, _battlecries, _deathrattles, _playReqs);
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

        public bool CanAttack() {
            return !JustPlayed && CurrentAttack > 0;
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

        private void Die() {
            Console.WriteLine("Card SID = " + ServerId + " died");
            CallDeathrattles();
        }

        private void CallDeathrattles() {
            _deathrattles?.Invoke(this);
        }

        public void Damage(int value) {
            CurrentHealth -= value;
            if(CurrentHealth <= 0)
                Die();
        }

        public bool CheckRequirement(PlayRequirement req) {
            if (_playReqs.TryGetValue(req, out bool needReq))
                return needReq;
            
            throw new RequirementException("Requerimento não encontrado.");
        }

    }
}
