using System;
using UnityEngine;
using UnityEngine.XR;

namespace CoopScoop.NewInputSystem
{
    public class ButtonPressedEventArgs : EventArgs
    {
        public float value = 0.0f;

        public ButtonPressedEventArgs(float value) => this.value = value;
    }

    public class ButtonReleasedEventArgs : EventArgs
    {
        public float value = 0.0f;

        public ButtonReleasedEventArgs(float value) => this.value = value;
    }

    public class VRAction
    {
        private InputFeatureUsage<float> _input;

        private float _prevValue = 0.0f;

        public delegate void PressedHandler(ButtonPressedEventArgs e);
        public event PressedHandler Pressed = delegate { };

        public delegate void ReleasedHandler(ButtonReleasedEventArgs e);
        public event ReleasedHandler Released = delegate { };

        public VRAction(InputFeatureUsage<float> input, PressedHandler pressedFunction, ReleasedHandler releasedFunction)
        {
            _input = input;

            Pressed += pressedFunction;
            Released += releasedFunction;
        }

        ~VRAction()
        {
            foreach (Delegate d in Pressed.GetInvocationList())
                Pressed -= (PressedHandler)d;
            foreach (Delegate d in Released.GetInvocationList())
                Released -= (ReleasedHandler)d;
        }

        public void Update(InputDevice device)
        {
            device.TryGetFeatureValue(_input, out float value);

            if (value > _prevValue && _prevValue == 0.0f)
                Press(new ButtonPressedEventArgs(value));
            else if (value == 0.0f && value < _prevValue)
                Release(new ButtonReleasedEventArgs(value));
            _prevValue = value;
        }

        public void Press(ButtonPressedEventArgs e) => Pressed.Invoke(e);
        public void Release(ButtonReleasedEventArgs e) => Released.Invoke(e);
    }
}