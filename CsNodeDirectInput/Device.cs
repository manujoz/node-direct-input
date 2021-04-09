using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;

namespace CsNodeDirectInput
{
    class Device
    {
        public DeviceInstance Instace;
        public Keyboard Keyboard = null;
        public Joystick Joystick = null;

        /**
         * Devuelce los detalles del dispositivo
         */
        public DeviceDetails GetDetails()
        {
            DeviceDetails details = new DeviceDetails();
            details.productId = Instace.ProductGuid.ToString();
            details.instaceId = Instace.InstanceGuid.ToString();
            details.name = Instace.ProductName;
            details.type = Instace.Type.ToString();
            details.subtype = Instace.Subtype.ToString();
            details.isHid = Instace.IsHumanInterfaceDevice;

            return details;
        }

        /**
         * Obtiene las teclas presionadas de los dispositivos conectados
         */
        public IList<DevicePresedKey> GetPressedKeys()
        {
            IList<DevicePresedKey> pressedKeys = new List<DevicePresedKey>();

            if (Keyboard != null)
            {
                try
                {
                    Keyboard.Poll();
                    KeyboardState state = Keyboard.GetCurrentState();

                    foreach (Key key in state.PressedKeys)
                    {
                        DevicePresedKey dpk = new DevicePresedKey();
                        dpk.deviceId = Instace.ProductGuid.ToString();
                        dpk.deviceName = Instace.ProductName;
                        dpk.key = key.ToString();
                        pressedKeys.Add(dpk);
                    }
                } catch
                {
                    //Console.WriteLine("El dispositivo " + Instace.ProductName + " ha sido desconectado");
                }
                
            }

            if (Joystick != null)
            {
                try
                {
                    Joystick.Poll();
                    JoystickState state = Joystick.GetCurrentState();

                    bool[] buttons = state.Buttons;
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i])
                        {
                            DevicePresedKey dpk = new DevicePresedKey();
                            dpk.deviceId = Instace.ProductGuid.ToString();
                            dpk.deviceName = Instace.ProductName;
                            dpk.key = i.ToString();
                            pressedKeys.Add(dpk);
                        }
                    }
                } catch
                {
                    //Console.WriteLine("El dispositivo " + Instace.ProductName + " ha sido desconectado");
                }
            }

            return pressedKeys;
        }
    }

    class DeviceDetails
    {
        public string productId;
        public string instaceId;
        public string name;
        public string type;
        public string subtype;
        public bool isHid;
    }

    class DevicePresedKey
    {
        public string deviceId;
        public string deviceName;
        public string key;
    }
}
