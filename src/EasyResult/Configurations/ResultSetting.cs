using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace EasyResult.Configurations
{
    internal static class ResultSetting
    {
        public static ResultOptions Options { get; set; }
       

        public static void Setup(Action<ResultOptions>? action)
        {
            action ??= _ => new ResultOptions();
            Options = new ResultOptions();
           action.Invoke(Options);

        }

    }
}
