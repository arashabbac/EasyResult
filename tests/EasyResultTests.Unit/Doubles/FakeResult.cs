using System.Collections.Generic;

namespace EasyResultTests.Unit.Doubles;

public class FakeResult
{
    public FakeResult()
    {
        Successes = new();
        Errors = new(); 
    }
    public bool IsSuccess { get; set; }
    public List<string> Successes { get; set; }
    public List<string> Errors { get; set; }
}

public class FakeResult<TData> : FakeResult where TData : class
{
    public TData? Data { get; set; }
}
