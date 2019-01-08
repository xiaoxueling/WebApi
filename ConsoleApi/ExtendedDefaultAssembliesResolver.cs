using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace FSTConsoleApi
{
    public class ExtendedDefaultAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            List<string> assemblyNames = new List<string>()
            {
                "FSTWebApi.dll"
            };
            if (null != assemblyNames)
            {
                foreach (var item in assemblyNames)
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(item);
                    if (!AppDomain.CurrentDomain.GetAssemblies().Any<Assembly>(assembly => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), assemblyName)))
                    {
                        AppDomain.CurrentDomain.Load(assemblyName);
                    }
                }
            }
            return base.GetAssemblies();
        }

    }
}
