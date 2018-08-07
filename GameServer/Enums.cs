
namespace GameServer {
    public static class Enums {
        public enum ServerPackets {
            WELCOME = 1,
            MESSAGE,
            KICK,
            SEND_IN_GAME,
            LOGIN_RESPONSE,
            CREATE_CARD,
            SET_TURN,
            SET_TOTAL_MANA,
            SET_AVAILABLE_MANA,
            SET_CAN_PLAY_CARD,
            SET_CAN_ATTACK,
            START_SELECT_TARGET,
            SET_CAN_BE_TARGET,
            END_SELECT_TARGET,
            UPDATE_CARD_HEALTH,
            DESTROY_CARD,
            DISPLAY_SPELL
        }

        public enum ClientPackets {
            NEW_ACCOUNT = 1,
            LOGIN,
            PLAY_CARD,
            END_TURN,
            PLAYER_READY,
            DRAW_CARDS,
            REQUEST_ATTACK_EVENT,
            TARGET_SELECT
        }

        public enum LoginResponse {
            DOES_NOT_EXISTS = 1,
            INCORRECT_PASSWORD,
            OK
        }

        public enum CardPlace {
            HAND,
            BOARD,
            ENEMY_BOARD,
            DECK,
            GRAVEYARD
        }

        public enum PlayRequirement {
            MINIONS_ON_BOARD,
            ENEMIES_ON_BOARD
        }
    }
}

