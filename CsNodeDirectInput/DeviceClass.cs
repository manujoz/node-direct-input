using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;
using Newtonsoft.Json;

namespace CsNodeDirectInput
{
    public class DeviceClass
    {
        DirectInput Di = new DirectInput();
        IList<Device> Devices = new List<Device>();

        public DeviceClass()
        {
            SearchConnectedDevices();
        }

        /**
         * Obtiene los disposivos conectados al equipo
         */
        public string GetDevices()
        {
            IList<DeviceDetails> details = new List<DeviceDetails>();
            foreach (Device dev in Devices)
            {
                details.Add(dev.GetDetails());
            }

            return JsonConvert.SerializeObject(details);
        }

        /**
         * Devuelve las teclas presionadas de los diferentes dispositivos conectados
         */
        public string GetPressedKeys()
        {
            IList<DevicePresedKey> pressedKeys = new List<DevicePresedKey>();

            foreach (Device dev in Devices)
            {
                IList<DevicePresedKey> pk = dev.GetPressedKeys();
                pressedKeys = pressedKeys.Concat(pk).ToList();
            }

            return JsonConvert.SerializeObject(pressedKeys);
        }

        /**
         * Obtiene los dispositivos conectacos
         */
        public void SearchConnectedDevices()
        {
            IList<DeviceInstance> devs = Di.GetDevices();

            foreach (DeviceInstance di in devs)
            {
                if (di.Type.ToString() == "Driving" || di.Type.ToString() == "Gamepad" || di.Type.ToString() == "Joystick" || di.Type.ToString() == "Flight" || di.Type.ToString() == "Keyboard")
                {
                    if (!isInDevices(di))
                    {
                        Device device = new Device();
                        device.Instace = di;

                        if (di.Type.ToString() == "Keyboard")
                        {
                            device.Keyboard = new Keyboard(Di);
                            device.Keyboard.Acquire();
                            device.GetPressedKeys();
                        }
                        else
                        {
                            device.Joystick = new Joystick(Di, di.InstanceGuid);
                            device.Joystick.Acquire();
                        }

                        //Console.WriteLine("Se elimina el dispositivo " + di.ProductName);
                        Devices.Add(device);
                    }
                }
            }

            RemoveDisconnectedDevices();
        }

        /**
         * Determina si un dispositivo está en la lista de devices
         */
        private bool isInDevices(DeviceInstance di)
        {
            foreach (Device dv in Devices)
            {
                if (dv.Instace.InstanceGuid == di.InstanceGuid)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Elimina los dispositivos que ya no están conectados
         */
        private void RemoveDisconnectedDevices()
        {
            IList<DeviceInstance> devs = Di.GetDevices();

            for (int i = 0; i < Devices.Count; i++)
            {
                bool exist = false;
                foreach (DeviceInstance dev in devs)
                {
                    if (dev.InstanceGuid == Devices[i].Instace.InstanceGuid)
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    //Console.WriteLine("Se elimina el dispositivo " + Devices[i].Instace.ProductName);
                    Devices.Remove(Devices[i]);
                }
            }

        }
    }
}
