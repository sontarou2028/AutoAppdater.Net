using System.Runtime.InteropServices;
using System.Security.Cryptography;
using AutoAppdater.Command;
using AutoAppdater.Interface;

namespace AutoAppdater.Mod
{
    internal static class Mod
    {
        internal class Comm
        {
            internal void Init()
            {
                ICommandComponent c1 = new ICommandComponent("list", CommandType.Word);
                ICommandComponent c0 = new ICommandComponent("mod", CommandType.Word,[c1]);
                c1.Handler.CallStepEventHandler1 += List;
                CommandSet.RegistCommandComponent(c0);
            }
            CommandResponse? List(string?[] value)
            {
                
            }
        }
    }
}