using System.Runtime.Serialization;

namespace Payslip.Application.Base
{
    [Serializable]
    public class ManagedException : Exception
    {
        public ManagedException()
        {
        }

        public ManagedException(string message) : base(message)
        {
        }

        public ManagedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ManagedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
