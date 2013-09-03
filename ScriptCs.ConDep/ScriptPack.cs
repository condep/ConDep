using System.Linq;
using ScriptCs.Contracts;

namespace ScriptCs.ConDep
{
    public class ScriptPack : IScriptPack
    {
        void IScriptPack.Initialize(IScriptPackSession session)
        {
            var namespaces = new[]
                {
                    "ConDep.Dsl",
                    "ConDep.Dsl.Config"
                }.ToList();

            namespaces.ForEach(session.ImportNamespace);
        }

        IScriptPackContext IScriptPack.GetContext()
        {
            return new ConDepPack();
        }

        void IScriptPack.Terminate()
        {
        }
    }
}
