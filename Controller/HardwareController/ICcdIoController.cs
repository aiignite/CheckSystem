namespace Controller.HardwareController
{
    public interface ICcdIoController
    {
        bool SetOutputs();

        void GetInputs();

        void ResetOutPuts();
    }
}
