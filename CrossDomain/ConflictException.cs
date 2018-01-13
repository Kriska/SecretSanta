using System;

namespace SecretSanta.CrossDomain
{
    public class ConflictException : Exception
    {
        public ConflictException(string Message) : base(Message)
        {
        }

        public ConflictException(Exception Exc) : base(Exc.Message, Exc)
        {
        }
    }
}