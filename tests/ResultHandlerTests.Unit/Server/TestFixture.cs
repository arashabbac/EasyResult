using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;

namespace ResultHandlerTests.Unit.Server;
public class TestFixture
{
    public TestServer Server;

    public TestFixture()
    {
        Server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
            .UseConfiguration(new ConfigurationBuilder().Build()
            )
            .UseStartup<Startup>());
    }
}
