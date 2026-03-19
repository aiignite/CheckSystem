using CommonUtility.BusLoader;

namespace Controller
{
    public sealed class LDLL16ENTR : ControllerBase
    {
        public CanBus CanFD;

        public LDLL16ENTR(string name) : base(name)
        {
        }

        ~LDLL16ENTR()
        {
            Dispose();
        }
    }
}
