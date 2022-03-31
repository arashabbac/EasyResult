using System.Net;

namespace ResultHandler.Configurations;

public interface IExceptionResult<T> where T : Exception
{
    void Configure(ExceptionResultBuilder<T> builder);
}
