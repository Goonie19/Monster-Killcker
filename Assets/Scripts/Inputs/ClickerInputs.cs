// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs/ClickerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ClickerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ClickerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ClickerInputs"",
    ""maps"": [
        {
            ""name"": ""Clicker"",
            ""id"": ""4e2f6cbc-6093-4d3a-815f-66e44e2d8e5e"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""26af4aa7-21a0-49ec-ae35-326a27c8717a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""890776e6-6397-44ab-914a-f62823a6c318"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bb7c4f60-d543-41d2-a7f4-49e8a324ee4a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44a55237-4f6c-4f18-b42d-d7efc19725cf"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Clicker
        m_Clicker = asset.FindActionMap("Clicker", throwIfNotFound: true);
        m_Clicker_Click = m_Clicker.FindAction("Click", throwIfNotFound: true);
        m_Clicker_MousePosition = m_Clicker.FindAction("MousePosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Clicker
    private readonly InputActionMap m_Clicker;
    private IClickerActions m_ClickerActionsCallbackInterface;
    private readonly InputAction m_Clicker_Click;
    private readonly InputAction m_Clicker_MousePosition;
    public struct ClickerActions
    {
        private @ClickerInputs m_Wrapper;
        public ClickerActions(@ClickerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_Clicker_Click;
        public InputAction @MousePosition => m_Wrapper.m_Clicker_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_Clicker; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClickerActions set) { return set.Get(); }
        public void SetCallbacks(IClickerActions instance)
        {
            if (m_Wrapper.m_ClickerActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_ClickerActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_ClickerActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_ClickerActionsCallbackInterface.OnClick;
                @MousePosition.started -= m_Wrapper.m_ClickerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_ClickerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_ClickerActionsCallbackInterface.OnMousePosition;
            }
            m_Wrapper.m_ClickerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public ClickerActions @Clicker => new ClickerActions(this);
    public interface IClickerActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
