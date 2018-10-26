/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.4
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class TemplateBinding : BaseComponent {
  internal new static TemplateBinding CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new TemplateBinding(cPtr, cMemoryOwn);
  }

  internal TemplateBinding(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(TemplateBinding obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public TemplateBinding() {
  }

  protected override System.IntPtr CreateCPtr(System.Type type, out bool registerExtend) {
    registerExtend = false;
    return NoesisGUI_PINVOKE.new_TemplateBinding__SWIG_0();
  }

  public TemplateBinding(DependencyProperty dp) : this(NoesisGUI_PINVOKE.new_TemplateBinding__SWIG_1(DependencyProperty.getCPtr(dp)), true) {
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
  }

  public DependencyProperty Property {
    set {
      NoesisGUI_PINVOKE.TemplateBinding_Property_set(swigCPtr, DependencyProperty.getCPtr(value));
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.TemplateBinding_Property_get(swigCPtr);
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public object ProvideValue(object targetObject, DependencyProperty targetProperty) {
    IntPtr cPtr = NoesisGUI_PINVOKE.TemplateBinding_ProvideValue(swigCPtr, Noesis.Extend.GetInstanceHandle(targetObject), DependencyProperty.getCPtr(targetProperty));
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return Noesis.Extend.GetProxy(cPtr, false);
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.TemplateBinding_GetStaticType();
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return ret;
  }

}

}
