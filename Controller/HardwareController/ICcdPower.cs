namespace Controller.HardwareController
{
   public interface ICcdPower
    {
        void ConnectPower(string protocolValue);

        void PowerOn();

        void PowerOff();

        void SetCombSerOn();

        void SetCombParaOn();

        void SetCombOff();

        void SetVoltage1(float voltage);

        void SetVoltage2(float voltage);

        void SetVoltage3(float voltage);

        void SetCurrent1(float current);

        void SetCurrent2(float current);

        void SetCurrent3(float current);

        void ReadCurrAndVolt();
    }
}
