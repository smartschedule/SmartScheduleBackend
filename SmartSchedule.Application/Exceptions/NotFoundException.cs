namespace SmartSchedule.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info,context)
        {

        }
    }
}
