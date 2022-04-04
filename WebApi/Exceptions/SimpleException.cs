using System.Net;
using EasyResult.Configurations;

namespace WebApi.Exceptions
{
    public class SimpleException: Exception, IExceptionResult<SimpleException>
    {
        public SimpleException()
        { }

        public SimpleException(string message)
            : base(message)
        { }

        public SimpleException(string message, Exception innerException)
            : base(message, innerException)
        { }
        public void Configure(ExceptionResultBuilder<SimpleException> builder)
        {
            builder.WithHttpStatusCode(HttpStatusCode.BadRequest);
        }
    }
}
