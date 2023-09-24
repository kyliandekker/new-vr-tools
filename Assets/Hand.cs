namespace CoopScoop.NewInputSystem
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;

    public class Hand : MonoBehaviour
    {
        private InputDevice _device;

        [SerializeField]
        private XRNode _handType;

        private List<VRAction> _vrActions = new List<VRAction>();

        private void OnEnable()
        {
            _vrActions.Add(new VRAction(CommonUsages.trigger, TriggerPressed, TriggerReleased));
            _vrActions.Add(new VRAction(CommonUsages.grip, GripPressed, GripReleased));
        }

        private void FindDevice()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(_handType, devices);
            if (devices.Count > 0)
                _device = devices[0];
        }

        private void Update()
        {
            if (!_device.isValid)
                FindDevice();

            for (int i = 0; i < _vrActions.Count; i++)
                _vrActions[i].Update(_device);
        }

        private void TriggerPressed(ButtonPressedEventArgs e)
        {
            Debug.Log("Trigger pressed");
        }

        private void TriggerReleased(ButtonReleasedEventArgs e)
        {
            Debug.Log("Trigger released");
        }

        private void GripPressed(ButtonPressedEventArgs e)
        {
            Debug.Log("Grabbing");
        }

        private void GripReleased(ButtonReleasedEventArgs e)
        {
            Debug.Log("Released");
        }
    }
}