using System;

namespace SecretSanta.CrossDomain
{
    public class UnautorizedException : Exception
    {
        public UnautorizedException(string Message) : base(Message)
        {
        }

        public UnautorizedException(Exception Exc) : base(Exc.Message, Exc)
        {
        }
    }
}