using System;

namespace SecretSanta.CrossDomain
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string Message) : base(Message)
        {
        }

        public NotFoundException(Exception Exc) : base(Exc.Message, Exc)
        {
        }
    }
}