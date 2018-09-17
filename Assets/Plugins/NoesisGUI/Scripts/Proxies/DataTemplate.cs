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

public class DataTemplate : FrameworkTemplate {
  internal new static DataTemplate CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new DataTemplate(cPtr, cMemoryOwn);
  }

  internal DataTemplate(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(DataTemplate obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public DataTemplate() {
  }

  protected override System.IntPtr CreateCPtr(System.Type type, out bool registerExtend) {
    registerExtend = false;
    return NoesisGUI_PINVOKE.new_DataTemplate();
  }

  public System.Type DataType {
    set {
      NoesisGUI_PINVOKE.DataTemplate_DataType_set(swigCPtr, new HandleRef(value, (value != null ? Noesis.Extend.GetNativeType(value) : IntPtr.Zero)));
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
    }
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.DataTemplate_DataType_get(swigCPtr);
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
      if (cPtr != IntPtr.Zero) {
        Noesis.Extend.NativeTypeInfo info = Noesis.Extend.GetNativeTypeInfo(cPtr);
        return info.Type;
      }
      return null;
    }
  }

  public TriggerCollection Triggers {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.DataTemplate_Triggers_get(swigCPtr);
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
      return (TriggerCollection)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.DataTemplate_GetStaticType();
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return ret;
  }

}

}

