using Ninject.Modules;
using YADA.DataAccess;

namespace YADA
{
    internal class MainModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<Reader>().To<YadaReader>();
            Kernel.Bind<Database>().ToSelf();
        }
    }
}