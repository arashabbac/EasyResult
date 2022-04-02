using System.Net;

namespace EasyResult.Configurations;

public interface IExceptionResult<T> where T : Exception
{
    void Configure(ExceptionResultBuilder<T> builder);
}
