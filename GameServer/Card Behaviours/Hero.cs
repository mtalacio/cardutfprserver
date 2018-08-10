namespace GameServer.Card_Behaviours {
    public class Hero : CardModel {

        public Hero(int id, int health, int attack, int mana, bool isTaunt) : base(id, health, attack, mana, isTaunt) {
            PlayRequirements = null;
        }
    }
}
