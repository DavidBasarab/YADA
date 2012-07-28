using Ninject.Modules;
using YADA.DataAccess;

namespace YADA
{
    internal class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Reader>().To<YadaReader>();
        }
    }
}