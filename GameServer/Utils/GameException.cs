﻿using System;

namespace GameServer.Utils {
    public class AccountExistsException : Exception {
        public AccountExistsException() : base("A conta requerida já existe.") { }
    }

    public class EndOfRecordsetException : Exception {
        public EndOfRecordsetException() : base("O cursor do Recordset já chegou ao final da tabela.") { }
    }

    public class AccountDoesNotExist : Exception {
        public AccountDoesNotExist() : base("A conta acessada não existe.") { }
    }

    public class IncorrectPasswordException : Exception {
        public IncorrectPasswordException() : base("Senha incorreta.") { }
    }

    public class IllegalMessageReceivedException : Exception {
        public IllegalMessageReceivedException() : base("Uma mensagem ilegal foi recebida pelo servidor!") { }
        public IllegalMessageReceivedException(string message) : base(message) { }
    }

    public class IllegalGameEventException : Exception {
        public IllegalGameEventException(string message) : base(message) { }
    }

    public class BehaviourNotFoundException : Exception {
        public BehaviourNotFoundException() : base("Carta não encontrada no banco de dados") { }
    }

    public class CardNotFoundException : Exception {
        public CardNotFoundException() : base("Carta não encontrada no jogo") { }
    }

    public class RequirementException : Exception {
        public RequirementException(string message) : base(message) { }
    }
}
