using System;
using System.Collections.Generic;

namespace StateMachine
{
    public abstract class CodeLine
    {
        protected DeviceConfig DeviceConfig { get; set; }

        protected static List<object> ControllersList { get; set; }

        protected string CodeStr { get; set; }

        public abstract Action Analyze();

        public abstract Action Analyze(object obj);

        public abstract void Invoke();

        protected CodeLine(
            List<object> controllersList, DeviceConfig deviceConfig)
        {
            ControllersList = controllersList;
            DeviceConfig = deviceConfig;
        }
    }
}
