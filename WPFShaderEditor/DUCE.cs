// Decompiled with JetBrains decompiler
// Type: System.Windows.Media.Composition.DUCE
// Assembly: PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CD7B53CF-E517-42D9-B6D9-F989F6E5A5A3
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\PresentationCore\v4.0_4.0.0.0__31bf3856ad364e35\PresentationCore.dll

using MS.Internal;
using MS.Internal.Interop;
using MS.Utility;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Decompiled_System.Windows.Media.Composition
{
  internal class DUCE
  {
    internal const uint waitInfinite = 4294967295;

    [SecurityCritical]
    internal static unsafe void CopyBytes(byte* pbTo, byte* pbFrom, int cbData)
    {
      int* numPtr1 = (int*) pbFrom;
      int* numPtr2 = (int*) pbTo;
      for (int index = 0; index < cbData / 4; ++index)
        numPtr2[index] = numPtr1[index];
    }

    [SecuritySafeCritical]
    internal static unsafe void NotifyPolicyChangeForNonInteractiveMode(
      bool forceRender,
      DUCE.Channel channel)
    {
      DUCE.MILCMD_PARTITION_NOTIFYPOLICYCHANGEFORNONINTERACTIVEMODE notifypolicychangefornoninteractivemode = new DUCE.MILCMD_PARTITION_NOTIFYPOLICYCHANGEFORNONINTERACTIVEMODE()
      {
        Type = MILCMD.MilCmdPartitionNotifyPolicyChangeForNonInteractiveMode,
        ShouldRenderEvenWhenNoDisplayDevicesAreAvailable = forceRender ? 1U : 0U
      };
      channel.SendCommand((byte*) &notifypolicychangefornoninteractivemode, sizeof (DUCE.MILCMD_PARTITION_NOTIFYPOLICYCHANGEFORNONINTERACTIVEMODE), false);
    }

    [SecurityCritical(SecurityCriticalScope.Everything)]
    [SuppressUnmanagedCodeSecurity]
    private static class UnsafeNativeMethods
    {
      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilResource_CreateOrAddRefOnChannel(
        IntPtr pChannel,
        DUCE.ResourceType resourceType,
        ref DUCE.ResourceHandle hResource);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilResource_DuplicateHandle(
        IntPtr pSourceChannel,
        DUCE.ResourceHandle original,
        IntPtr pTargetChannel,
        ref DUCE.ResourceHandle duplicate);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilConnection_CreateChannel(
        IntPtr pTransport,
        IntPtr hChannel,
        out IntPtr channelHandle);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilConnection_DestroyChannel(IntPtr channelHandle);

      [DllImport("wpfgfx_v0400.dll", EntryPoint = "MilChannel_CloseBatch")]
      internal static extern int MilConnection_CloseBatch(IntPtr channelHandle);

      [DllImport("wpfgfx_v0400.dll", EntryPoint = "MilChannel_CommitChannel")]
      internal static extern int MilConnection_CommitChannel(IntPtr channelHandle);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int WgxConnection_SameThreadPresent(IntPtr pConnection);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilChannel_GetMarshalType(
        IntPtr channelHandle,
        out ChannelMarshalType marshalType);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern unsafe int MilResource_SendCommand(
        byte* pbData,
        uint cbSize,
        bool sendInSeparateBatch,
        IntPtr pChannel);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern unsafe int MilChannel_BeginCommand(
        IntPtr pChannel,
        byte* pbData,
        uint cbSize,
        uint cbExtra);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern unsafe int MilChannel_AppendCommandData(
        IntPtr pChannel,
        byte* pbData,
        uint cbSize);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilChannel_EndCommand(IntPtr pChannel);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilResource_SendCommandMedia(
        DUCE.ResourceHandle handle,
        SafeMediaHandle pMedia,
        IntPtr pChannel,
        bool notifyUceDirect);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilResource_SendCommandBitmapSource(
        DUCE.ResourceHandle handle,
        BitmapSourceSafeMILHandle pBitmapSource,
        IntPtr pChannel);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilResource_ReleaseOnChannel(
        IntPtr pChannel,
        DUCE.ResourceHandle hResource,
        out int deleted);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilChannel_SetNotificationWindow(
        IntPtr pChannel,
        IntPtr hwnd,
        WindowMessage message);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilComposition_WaitForNextMessage(
        IntPtr pChannel,
        int nCount,
        IntPtr[] handles,
        int bWaitAll,
        uint waitTimeout,
        out int waitReturn);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilComposition_PeekNextMessage(
        IntPtr pChannel,
        out DUCE.MilMessage.Message message,
        IntPtr messageSize,
        out int messageRetrieved);

      [DllImport("wpfgfx_v0400.dll")]
      internal static extern int MilResource_GetRefCountOnChannel(
        IntPtr pChannel,
        DUCE.ResourceHandle hResource,
        out uint refCount);
    }

    internal static class MilMessage
    {
      internal enum Type
      {
        ForceDWORD = -1, // 0xFFFFFFFF
        Invalid = 0,
        SyncFlushReply = 1,
        Caps = 4,
        PartitionIsZombie = 6,
        SyncModeStatus = 9,
        Presented = 10, // 0x0000000A
        BadPixelShader = 16, // 0x00000010
      }

      [StructLayout(LayoutKind.Explicit, Pack = 1)]
      internal struct CapsData
      {
        [FieldOffset(0)]
        internal int CommonMinimumCaps;
        [FieldOffset(4)]
        internal uint DisplayUniqueness;
        [FieldOffset(8)]
        internal MilGraphicsAccelerationCaps Caps;
      }

      [StructLayout(LayoutKind.Explicit, Pack = 1)]
      internal struct PartitionIsZombieStatus
      {
        [FieldOffset(0)]
        internal int HRESULTFailureCode;
      }

      [StructLayout(LayoutKind.Explicit, Pack = 1)]
      internal struct SyncModeStatus
      {
        [FieldOffset(0)]
        internal int Enabled;
      }

      [StructLayout(LayoutKind.Explicit, Pack = 1)]
      internal struct Presented
      {
        [FieldOffset(0)]
        internal MIL_PRESENTATION_RESULTS PresentationResults;
        [FieldOffset(4)]
        internal int RefreshRate;
        [FieldOffset(8)]
        internal long PresentationTime;
      }

      [StructLayout(LayoutKind.Explicit, Pack = 1)]
      internal struct Message
      {
        [FieldOffset(0)]
        internal DUCE.MilMessage.Type Type;
        [FieldOffset(4)]
        internal int Reserved;
        [FieldOffset(8)]
        internal DUCE.MilMessage.CapsData Caps;
        [FieldOffset(8)]
        internal DUCE.MilMessage.PartitionIsZombieStatus HRESULTFailure;
        [FieldOffset(8)]
        internal DUCE.MilMessage.Presented Presented;
        [FieldOffset(8)]
        internal DUCE.MilMessage.SyncModeStatus SyncModeStatus;
      }
    }

    internal struct ChannelSet
    {
      internal DUCE.Channel Channel;
      internal DUCE.Channel OutOfBandChannel;
    }

    internal sealed class Channel
    {
      [SecurityCritical]
      private IntPtr _hChannel;
      private DUCE.Channel _referenceChannel;
      private bool _isSynchronous;
      private bool _isOutOfBandChannel;
      private IntPtr _pConnection;

      [SecurityCritical]
      public Channel(
        DUCE.Channel referenceChannel,
        bool isOutOfBandChannel,
        IntPtr pConnection,
        bool isSynchronous)
      {
        IntPtr hChannel = IntPtr.Zero;
        this._referenceChannel = referenceChannel;
        this._pConnection = pConnection;
        this._isOutOfBandChannel = isOutOfBandChannel;
        this._isSynchronous = isSynchronous;
        if (referenceChannel != null)
          hChannel = referenceChannel._hChannel;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilConnection_CreateChannel(this._pConnection, hChannel, out this._hChannel));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal void Commit()
      {
        if (this._hChannel == IntPtr.Zero)
          return;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilConnection_CommitChannel(this._hChannel));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal void CloseBatch()
      {
        if (this._hChannel == IntPtr.Zero)
          return;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilConnection_CloseBatch(this._hChannel));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal void SyncFlush()
      {
        if (this._hChannel == IntPtr.Zero)
          return;
        MS.Internal.HRESULT.Check(MilCoreApi.MilComposition_SyncFlush(this._hChannel));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal void Close()
      {
        if (this._hChannel != IntPtr.Zero)
        {
          MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilConnection_CloseBatch(this._hChannel));
          MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilConnection_CommitChannel(this._hChannel));
        }
        this._referenceChannel = (DUCE.Channel) null;
        if (!(this._hChannel != IntPtr.Zero))
          return;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilConnection_DestroyChannel(this._hChannel));
        this._hChannel = IntPtr.Zero;
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal void Present() => MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.WgxConnection_SameThreadPresent(this._pConnection));

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal bool CreateOrAddRefOnChannel(
        object instance,
        ref DUCE.ResourceHandle handle,
        DUCE.ResourceType resourceType)
      {
        bool isNull = handle.IsNull;
        Invariant.Assert(this._hChannel != IntPtr.Zero);
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilResource_CreateOrAddRefOnChannel(this._hChannel, resourceType, ref handle));
        if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordGraphics, EventTrace.Level.PERF_LOW))
        {
          int num = (int) EventTrace.EventProvider.TraceEvent(EventTrace.Event.CreateOrAddResourceOnChannel, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordGraphics, EventTrace.Level.PERF_LOW, (object) PerfService.GetPerfElementID(instance), (object) this._hChannel, (object) (uint) handle, (object) (uint) resourceType);
        }
        return isNull;
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal DUCE.ResourceHandle DuplicateHandle(
        DUCE.ResourceHandle original,
        DUCE.Channel targetChannel)
      {
        DUCE.ResourceHandle duplicate = DUCE.ResourceHandle.Null;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilResource_DuplicateHandle(this._hChannel, original, targetChannel._hChannel, ref duplicate));
        return duplicate;
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal bool ReleaseOnChannel(DUCE.ResourceHandle handle)
      {
        Invariant.Assert(this._hChannel != IntPtr.Zero);
        int deleted;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilResource_ReleaseOnChannel(this._hChannel, handle, out deleted));
        if (deleted != 0 && EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordGraphics, EventTrace.Level.PERF_LOW))
        {
          int num = (int) EventTrace.EventProvider.TraceEvent(EventTrace.Event.ReleaseOnChannel, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordGraphics, EventTrace.Level.PERF_LOW, (object) this._hChannel, (object) (uint) handle);
        }
        return (uint) deleted > 0U;
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal uint GetRefCount(DUCE.ResourceHandle handle)
      {
        Invariant.Assert(this._hChannel != IntPtr.Zero);
        uint refCount;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilResource_GetRefCountOnChannel(this._hChannel, handle, out refCount));
        return refCount;
      }

      internal bool IsConnected
      {
        [SecurityCritical, SecurityTreatAsSafe] get => MediaContext.CurrentMediaContext.IsConnected;
      }

      internal ChannelMarshalType MarshalType
      {
        [SecurityCritical, SecurityTreatAsSafe] get
        {
          Invariant.Assert(this._hChannel != IntPtr.Zero);
          ChannelMarshalType marshalType;
          MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilChannel_GetMarshalType(this._hChannel, out marshalType));
          return marshalType;
        }
      }

      internal bool IsSynchronous => this._isSynchronous;

      internal bool IsOutOfBandChannel => this._isOutOfBandChannel;

      [SecurityCritical]
      internal unsafe void SendCommand(byte* pCommandData, int cSize) => this.SendCommand(pCommandData, cSize, false);

      [SecurityCritical]
      internal unsafe void SendCommand(byte* pCommandData, int cSize, bool sendInSeparateBatch)
      {
        Invariant.Assert((IntPtr) pCommandData != IntPtr.Zero && cSize > 0);
        if (this._hChannel == IntPtr.Zero)
          return;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilResource_SendCommand(pCommandData, checked ((uint) cSize), sendInSeparateBatch, this._hChannel));
      }

      [SecurityCritical]
      internal unsafe void BeginCommand(byte* pbCommandData, int cbSize, int cbExtra)
      {
        Invariant.Assert(cbSize > 0);
        if (this._hChannel == IntPtr.Zero)
          return;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilChannel_BeginCommand(this._hChannel, pbCommandData, checked ((uint) cbSize), checked ((uint) cbExtra)));
      }

      [SecurityCritical]
      internal unsafe void AppendCommandData(byte* pbCommandData, int cbSize)
      {
        Invariant.Assert((IntPtr) pbCommandData != IntPtr.Zero && cbSize > 0);
        if (this._hChannel == IntPtr.Zero)
          return;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilChannel_AppendCommandData(this._hChannel, pbCommandData, checked ((uint) cbSize)));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal void EndCommand()
      {
        if (this._hChannel == IntPtr.Zero)
          return;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilChannel_EndCommand(this._hChannel));
      }

      [SecurityCritical]
      internal void SendCommandBitmapSource(
        DUCE.ResourceHandle imageHandle,
        BitmapSourceSafeMILHandle pBitmapSource)
      {
        Invariant.Assert(pBitmapSource != null && !pBitmapSource.IsInvalid);
        Invariant.Assert(this._hChannel != IntPtr.Zero);
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilResource_SendCommandBitmapSource(imageHandle, pBitmapSource, this._hChannel));
      }

      [SecurityCritical]
      internal void SendCommandMedia(
        DUCE.ResourceHandle mediaHandle,
        SafeMediaHandle pMedia,
        bool notifyUceDirect)
      {
        Invariant.Assert(pMedia != null && !pMedia.IsInvalid);
        Invariant.Assert(this._hChannel != IntPtr.Zero);
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilResource_SendCommandMedia(mediaHandle, pMedia, this._hChannel, notifyUceDirect));
      }

      [SecurityCritical]
      internal void SetNotificationWindow(IntPtr hwnd, WindowMessage message)
      {
        Invariant.Assert(this._hChannel != IntPtr.Zero);
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilChannel_SetNotificationWindow(this._hChannel, hwnd, message));
      }

      [SecurityCritical]
      internal void WaitForNextMessage() => MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilComposition_WaitForNextMessage(this._hChannel, 0, (IntPtr[]) null, 1, uint.MaxValue, out int _));

      [SecurityCritical]
      internal bool PeekNextMessage(out DUCE.MilMessage.Message message)
      {
        Invariant.Assert(this._hChannel != IntPtr.Zero);
        int messageRetrieved;
        MS.Internal.HRESULT.Check(DUCE.UnsafeNativeMethods.MilComposition_PeekNextMessage(this._hChannel, out message, (IntPtr) sizeof (DUCE.MilMessage.Message), out messageRetrieved));
        return (uint) messageRetrieved > 0U;
      }
    }

    internal struct Resource
    {
      public static readonly DUCE.Resource Null = new DUCE.Resource(DUCE.ResourceHandle.Null);
      private DUCE.ResourceHandle _handle;

      public Resource(DUCE.ResourceHandle h) => this._handle = h;

      public bool CreateOrAddRefOnChannel(
        object instance,
        DUCE.Channel channel,
        DUCE.ResourceType type)
      {
        return channel.CreateOrAddRefOnChannel(instance, ref this._handle, type);
      }

      public bool ReleaseOnChannel(DUCE.Channel channel)
      {
        if (!channel.ReleaseOnChannel(this._handle))
          return false;
        this._handle = DUCE.ResourceHandle.Null;
        return true;
      }

      public bool IsOnChannel(DUCE.Channel channel) => !this._handle.IsNull;

      public DUCE.ResourceHandle Handle => this._handle;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct ResourceHandle
    {
      public static readonly DUCE.ResourceHandle Null = new DUCE.ResourceHandle(0U);
      [FieldOffset(0)]
      private uint _handle;

      public static explicit operator uint(DUCE.ResourceHandle r) => r._handle;

      public ResourceHandle(uint handle) => this._handle = handle;

      public bool IsNull => this._handle == 0U;
    }

    internal struct Map<ValueType>
    {
      private const int FOUND_IN_INLINE_STORAGE = -1;
      private const int NOT_FOUND = -2;
      private DUCE.Map<ValueType>.Entry _first;
      private List<DUCE.Map<ValueType>.Entry> _others;

      public bool IsEmpty() => this._first._key == null && this._others == null;

      private int Find(object key)
      {
        int num = -2;
        if (this._first._key != null)
        {
          if (this._first._key == key)
            num = -1;
          else if (this._others != null)
          {
            for (int index = 0; index < this._others.Count; ++index)
            {
              if (this._others[index]._key == key)
              {
                num = index;
                break;
              }
            }
          }
        }
        return num;
      }

      public void Set(object key, ValueType value)
      {
        int index = this.Find(key);
        switch (index)
        {
          case -2:
            if (this._first._key == null)
            {
              this._first = new DUCE.Map<ValueType>.Entry(key, value);
              break;
            }
            if (this._others == null)
              this._others = new List<DUCE.Map<ValueType>.Entry>(2);
            this._others.Add(new DUCE.Map<ValueType>.Entry(key, value));
            break;
          case -1:
            this._first._value = value;
            break;
          default:
            this._others[index] = new DUCE.Map<ValueType>.Entry(key, value);
            break;
        }
      }

      public bool Remove(object key)
      {
        int index1 = this.Find(key);
        if (index1 == -1)
        {
          if (this._others != null)
          {
            int index2 = this._others.Count - 1;
            this._first = this._others[index2];
            if (index2 == 0)
              this._others = (List<DUCE.Map<ValueType>.Entry>) null;
            else
              this._others.RemoveAt(index2);
          }
          else
            this._first = new DUCE.Map<ValueType>.Entry();
          return true;
        }
        if (index1 < 0)
          return false;
        if (this._others.Count == 1)
          this._others = (List<DUCE.Map<ValueType>.Entry>) null;
        else
          this._others.RemoveAt(index1);
        return true;
      }

      public bool Get(object key, out ValueType value)
      {
        int index = this.Find(key);
        value = default (ValueType);
        if (index == -1)
        {
          value = this._first._value;
          return true;
        }
        if (index < 0)
          return false;
        value = this._others[index]._value;
        return true;
      }

      public int Count()
      {
        if (this._first._key == null)
          return 0;
        return this._others == null ? 1 : this._others.Count + 1;
      }

      public object Get(int index)
      {
        if (index >= this.Count())
          return (object) null;
        return index == 0 ? this._first._key : this._others[index - 1]._key;
      }

      private struct Entry
      {
        public object _key;
        public ValueType _value;

        public Entry(object k, ValueType v)
        {
          this._key = k;
          this._value = v;
        }
      }
    }

    internal struct Map
    {
      private const int FOUND_IN_INLINE_STORAGE = -1;
      private const int NOT_FOUND = -2;
      private DUCE.Map.Entry _head;

      public bool IsEmpty() => this._head._key == null;

      private int Find(object key)
      {
        int num = -2;
        if (this._head._key != null)
        {
          if (this._head._key == key)
            num = -1;
          else if (this._head._value.IsNull)
          {
            List<DUCE.Map.Entry> key1 = (List<DUCE.Map.Entry>) this._head._key;
            for (int index = 0; index < key1.Count; ++index)
            {
              if (key1[index]._key == key)
              {
                num = index;
                break;
              }
            }
          }
        }
        return num;
      }

      public void Set(object key, DUCE.ResourceHandle value)
      {
        int index = this.Find(key);
        switch (index)
        {
          case -2:
            if (this._head._key == null)
            {
              this._head = new DUCE.Map.Entry(key, value);
              break;
            }
            if (!this._head._value.IsNull)
            {
              this._head._key = (object) new List<DUCE.Map.Entry>(2)
              {
                this._head,
                new DUCE.Map.Entry(key, value)
              };
              this._head._value = DUCE.ResourceHandle.Null;
              break;
            }
            ((List<DUCE.Map.Entry>) this._head._key).Add(new DUCE.Map.Entry(key, value));
            break;
          case -1:
            this._head._value = value;
            break;
          default:
            ((List<DUCE.Map.Entry>) this._head._key)[index] = new DUCE.Map.Entry(key, value);
            break;
        }
      }

      public bool Remove(object key)
      {
        int index = this.Find(key);
        if (index == -1)
        {
          this._head = new DUCE.Map.Entry();
          return true;
        }
        if (index < 0)
          return false;
        List<DUCE.Map.Entry> key1 = (List<DUCE.Map.Entry>) this._head._key;
        if (this.Count() == 2)
          this._head = key1[1 - index];
        else
          key1.RemoveAt(index);
        return true;
      }

      public bool Get(object key, out DUCE.ResourceHandle value)
      {
        int index = this.Find(key);
        value = DUCE.ResourceHandle.Null;
        if (index == -1)
        {
          value = this._head._value;
          return true;
        }
        if (index < 0)
          return false;
        value = ((List<DUCE.Map.Entry>) this._head._key)[index]._value;
        return true;
      }

      public int Count()
      {
        if (this._head._key == null)
          return 0;
        return !this._head._value.IsNull ? 1 : ((List<DUCE.Map.Entry>) this._head._key).Count;
      }

      public object Get(int index)
      {
        if (index >= this.Count() || index < 0)
          return (object) null;
        return this.Count() == 1 ? this._head._key : ((List<DUCE.Map.Entry>) this._head._key)[index]._key;
      }

      private struct Entry
      {
        public object _key;
        public DUCE.ResourceHandle _value;

        public Entry(object k, DUCE.ResourceHandle v)
        {
          this._key = k;
          this._value = v;
        }
      }
    }

    internal class ShareableDUCEMultiChannelResource
    {
      public DUCE.MultiChannelResource _duceResource;
    }

    internal struct MultiChannelResource
    {
      private DUCE.Map _map;

      public bool CreateOrAddRefOnChannel(
        object instance,
        DUCE.Channel channel,
        DUCE.ResourceType type)
      {
        DUCE.ResourceHandle handle;
        bool flag = this._map.Get((object) channel, out handle);
        bool orAddRefOnChannel = channel.CreateOrAddRefOnChannel(instance, ref handle, type);
        if (!flag)
          this._map.Set((object) channel, handle);
        return orAddRefOnChannel;
      }

      public DUCE.ResourceHandle DuplicateHandle(
        DUCE.Channel sourceChannel,
        DUCE.Channel targetChannel)
      {
        DUCE.ResourceHandle resourceHandle1 = DUCE.ResourceHandle.Null;
        DUCE.ResourceHandle original = DUCE.ResourceHandle.Null;
        this._map.Get((object) sourceChannel, out original);
        DUCE.ResourceHandle resourceHandle2 = sourceChannel.DuplicateHandle(original, targetChannel);
        if (!resourceHandle2.IsNull)
          this._map.Set((object) targetChannel, resourceHandle2);
        return resourceHandle2;
      }

      public bool ReleaseOnChannel(DUCE.Channel channel)
      {
        DUCE.ResourceHandle handle;
        this._map.Get((object) channel, out handle);
        if (!channel.ReleaseOnChannel(handle))
          return false;
        this._map.Remove((object) channel);
        return true;
      }

      public DUCE.ResourceHandle GetHandle(DUCE.Channel channel)
      {
        DUCE.ResourceHandle resourceHandle;
        if (channel != null)
          this._map.Get((object) channel, out resourceHandle);
        else
          resourceHandle = DUCE.ResourceHandle.Null;
        return resourceHandle;
      }

      public bool IsOnChannel(DUCE.Channel channel) => !this.GetHandle(channel).IsNull;

      public bool IsOnAnyChannel => !this._map.IsEmpty();

      public int GetChannelCount() => this._map.Count();

      public DUCE.Channel GetChannel(int index) => this._map.Get(index) as DUCE.Channel;

      public uint GetRefCountOnChannel(DUCE.Channel channel)
      {
        DUCE.ResourceHandle handle;
        this._map.Get((object) channel, out handle);
        return channel.GetRefCount(handle);
      }
    }

    internal static class CompositionNode
    {
      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetTransform(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hTransform,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETTRANSFORM visualSettransform;
        visualSettransform.Type = MILCMD.MilCmdVisualSetTransform;
        visualSettransform.Handle = hCompositionNode;
        visualSettransform.hTransform = hTransform;
        channel.SendCommand((byte*) &visualSettransform, sizeof (DUCE.MILCMD_VISUAL_SETTRANSFORM));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetEffect(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hEffect,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETEFFECT milcmdVisualSeteffect;
        milcmdVisualSeteffect.Type = MILCMD.MilCmdVisualSetEffect;
        milcmdVisualSeteffect.Handle = hCompositionNode;
        milcmdVisualSeteffect.hEffect = hEffect;
        channel.SendCommand((byte*) &milcmdVisualSeteffect, sizeof (DUCE.MILCMD_VISUAL_SETEFFECT));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetCacheMode(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hCacheMode,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETCACHEMODE visualSetcachemode;
        visualSetcachemode.Type = MILCMD.MilCmdVisualSetCacheMode;
        visualSetcachemode.Handle = hCompositionNode;
        visualSetcachemode.hCacheMode = hCacheMode;
        channel.SendCommand((byte*) &visualSetcachemode, sizeof (DUCE.MILCMD_VISUAL_SETCACHEMODE));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetOffset(
        DUCE.ResourceHandle hCompositionNode,
        double offsetX,
        double offsetY,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETOFFSET milcmdVisualSetoffset;
        milcmdVisualSetoffset.Type = MILCMD.MilCmdVisualSetOffset;
        milcmdVisualSetoffset.Handle = hCompositionNode;
        milcmdVisualSetoffset.offsetX = offsetX;
        milcmdVisualSetoffset.offsetY = offsetY;
        channel.SendCommand((byte*) &milcmdVisualSetoffset, sizeof (DUCE.MILCMD_VISUAL_SETOFFSET));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetContent(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hContent,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETCONTENT visualSetcontent;
        visualSetcontent.Type = MILCMD.MilCmdVisualSetContent;
        visualSetcontent.Handle = hCompositionNode;
        visualSetcontent.hContent = hContent;
        channel.SendCommand((byte*) &visualSetcontent, sizeof (DUCE.MILCMD_VISUAL_SETCONTENT));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetAlpha(
        DUCE.ResourceHandle hCompositionNode,
        double alpha,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETALPHA milcmdVisualSetalpha;
        milcmdVisualSetalpha.Type = MILCMD.MilCmdVisualSetAlpha;
        milcmdVisualSetalpha.Handle = hCompositionNode;
        milcmdVisualSetalpha.alpha = alpha;
        channel.SendCommand((byte*) &milcmdVisualSetalpha, sizeof (DUCE.MILCMD_VISUAL_SETALPHA));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetAlphaMask(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hAlphaMaskBrush,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETALPHAMASK visualSetalphamask;
        visualSetalphamask.Type = MILCMD.MilCmdVisualSetAlphaMask;
        visualSetalphamask.Handle = hCompositionNode;
        visualSetalphamask.hAlphaMask = hAlphaMaskBrush;
        channel.SendCommand((byte*) &visualSetalphamask, sizeof (DUCE.MILCMD_VISUAL_SETALPHAMASK));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetScrollableAreaClip(
        DUCE.ResourceHandle hCompositionNode,
        Rect? clip,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETSCROLLABLEAREACLIP setscrollableareaclip;
        setscrollableareaclip.Type = MILCMD.MilCmdVisualSetScrollableAreaClip;
        setscrollableareaclip.Handle = hCompositionNode;
        setscrollableareaclip.IsEnabled = clip.HasValue ? 1U : 0U;
        setscrollableareaclip.Clip = !clip.HasValue ? Rect.Empty : clip.Value;
        channel.SendCommand((byte*) &setscrollableareaclip, sizeof (DUCE.MILCMD_VISUAL_SETSCROLLABLEAREACLIP));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetClip(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hClip,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETCLIP milcmdVisualSetclip;
        milcmdVisualSetclip.Type = MILCMD.MilCmdVisualSetClip;
        milcmdVisualSetclip.Handle = hCompositionNode;
        milcmdVisualSetclip.hClip = hClip;
        channel.SendCommand((byte*) &milcmdVisualSetclip, sizeof (DUCE.MILCMD_VISUAL_SETCLIP));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetRenderOptions(
        DUCE.ResourceHandle hCompositionNode,
        MilRenderOptions renderOptions,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_SETRENDEROPTIONS setrenderoptions;
        setrenderoptions.Type = MILCMD.MilCmdVisualSetRenderOptions;
        setrenderoptions.Handle = hCompositionNode;
        setrenderoptions.renderOptions = renderOptions;
        channel.SendCommand((byte*) &setrenderoptions, sizeof (DUCE.MILCMD_VISUAL_SETRENDEROPTIONS));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void RemoveChild(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hChild,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_REMOVECHILD visualRemovechild;
        visualRemovechild.Type = MILCMD.MilCmdVisualRemoveChild;
        visualRemovechild.Handle = hCompositionNode;
        visualRemovechild.hChild = hChild;
        channel.SendCommand((byte*) &visualRemovechild, sizeof (DUCE.MILCMD_VISUAL_REMOVECHILD));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void RemoveAllChildren(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_REMOVEALLCHILDREN removeallchildren;
        removeallchildren.Type = MILCMD.MilCmdVisualRemoveAllChildren;
        removeallchildren.Handle = hCompositionNode;
        channel.SendCommand((byte*) &removeallchildren, sizeof (DUCE.MILCMD_VISUAL_REMOVEALLCHILDREN));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void InsertChildAt(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hChild,
        uint iPosition,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL_INSERTCHILDAT visualInsertchildat;
        visualInsertchildat.Type = MILCMD.MilCmdVisualInsertChildAt;
        visualInsertchildat.Handle = hCompositionNode;
        visualInsertchildat.hChild = hChild;
        visualInsertchildat.index = iPosition;
        channel.SendCommand((byte*) &visualInsertchildat, sizeof (DUCE.MILCMD_VISUAL_INSERTCHILDAT));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetGuidelineCollection(
        DUCE.ResourceHandle hCompositionNode,
        DoubleCollection guidelinesX,
        DoubleCollection guidelinesY,
        DUCE.Channel channel)
      {
        int num = guidelinesX == null ? 0 : guidelinesX.Count;
        int length1 = guidelinesY == null ? 0 : guidelinesY.Count;
        int length2 = checked (num + length1);
        DUCE.MILCMD_VISUAL_SETGUIDELINECOLLECTION setguidelinecollection;
        setguidelinecollection.Type = MILCMD.MilCmdVisualSetGuidelineCollection;
        setguidelinecollection.Handle = hCompositionNode;
        setguidelinecollection.countX = checked ((ushort) num);
        setguidelinecollection.countY = checked ((ushort) length1);
        if (num == 0 && length1 == 0)
        {
          channel.SendCommand((byte*) &setguidelinecollection, sizeof (DUCE.MILCMD_VISUAL_SETGUIDELINECOLLECTION));
        }
        else
        {
          double[] array = new double[length2];
          if (num != 0)
          {
            guidelinesX.CopyTo(array, 0);
            Array.Sort<double>(array, 0, num);
          }
          if (length1 != 0)
          {
            guidelinesY.CopyTo(array, num);
            Array.Sort<double>(array, num, length1);
          }
          float[] numArray = new float[length2];
          int index = 0;
          while (index < length2)
          {
            numArray[index] = (float) array[index];
            checked { ++index; }
          }
          channel.BeginCommand((byte*) &setguidelinecollection, sizeof (DUCE.MILCMD_VISUAL_SETGUIDELINECOLLECTION), checked (4 * length2));
          fixed (float* numPtr = numArray)
            channel.AppendCommandData((byte*) numPtr, checked (4 * length2));
          channel.EndCommand();
        }
      }
    }

    internal static class Viewport3DVisualNode
    {
      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetCamera(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hCamera,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VIEWPORT3DVISUAL_SETCAMERA dvisualSetcamera;
        dvisualSetcamera.Type = MILCMD.MilCmdViewport3DVisualSetCamera;
        dvisualSetcamera.Handle = hCompositionNode;
        dvisualSetcamera.hCamera = hCamera;
        channel.SendCommand((byte*) &dvisualSetcamera, sizeof (DUCE.MILCMD_VIEWPORT3DVISUAL_SETCAMERA));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetViewport(
        DUCE.ResourceHandle hCompositionNode,
        Rect viewport,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VIEWPORT3DVISUAL_SETVIEWPORT dvisualSetviewport;
        dvisualSetviewport.Type = MILCMD.MilCmdViewport3DVisualSetViewport;
        dvisualSetviewport.Handle = hCompositionNode;
        dvisualSetviewport.Viewport = viewport;
        channel.SendCommand((byte*) &dvisualSetviewport, sizeof (DUCE.MILCMD_VIEWPORT3DVISUAL_SETVIEWPORT));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void Set3DChild(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hVisual3D,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VIEWPORT3DVISUAL_SET3DCHILD dvisualSeT3Dchild;
        dvisualSeT3Dchild.Type = MILCMD.MilCmdViewport3DVisualSet3DChild;
        dvisualSeT3Dchild.Handle = hCompositionNode;
        dvisualSeT3Dchild.hChild = hVisual3D;
        channel.SendCommand((byte*) &dvisualSeT3Dchild, sizeof (DUCE.MILCMD_VIEWPORT3DVISUAL_SET3DCHILD));
      }
    }

    internal static class Visual3DNode
    {
      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void RemoveChild(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hChild,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL3D_REMOVECHILD visuaL3DRemovechild;
        visuaL3DRemovechild.Type = MILCMD.MilCmdVisual3DRemoveChild;
        visuaL3DRemovechild.Handle = hCompositionNode;
        visuaL3DRemovechild.hChild = hChild;
        channel.SendCommand((byte*) &visuaL3DRemovechild, sizeof (DUCE.MILCMD_VISUAL3D_REMOVECHILD));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void RemoveAllChildren(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL3D_REMOVEALLCHILDREN removeallchildren;
        removeallchildren.Type = MILCMD.MilCmdVisual3DRemoveAllChildren;
        removeallchildren.Handle = hCompositionNode;
        channel.SendCommand((byte*) &removeallchildren, sizeof (DUCE.MILCMD_VISUAL3D_REMOVEALLCHILDREN));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void InsertChildAt(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hChild,
        uint iPosition,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL3D_INSERTCHILDAT l3DInsertchildat;
        l3DInsertchildat.Type = MILCMD.MilCmdVisual3DInsertChildAt;
        l3DInsertchildat.Handle = hCompositionNode;
        l3DInsertchildat.hChild = hChild;
        l3DInsertchildat.index = iPosition;
        channel.SendCommand((byte*) &l3DInsertchildat, sizeof (DUCE.MILCMD_VISUAL3D_INSERTCHILDAT));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetContent(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hContent,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL3D_SETCONTENT visuaL3DSetcontent;
        visuaL3DSetcontent.Type = MILCMD.MilCmdVisual3DSetContent;
        visuaL3DSetcontent.Handle = hCompositionNode;
        visuaL3DSetcontent.hContent = hContent;
        channel.SendCommand((byte*) &visuaL3DSetcontent, sizeof (DUCE.MILCMD_VISUAL3D_SETCONTENT));
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetTransform(
        DUCE.ResourceHandle hCompositionNode,
        DUCE.ResourceHandle hTransform,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_VISUAL3D_SETTRANSFORM visuaL3DSettransform;
        visuaL3DSettransform.Type = MILCMD.MilCmdVisual3DSetTransform;
        visuaL3DSettransform.Handle = hCompositionNode;
        visuaL3DSettransform.hTransform = hTransform;
        channel.SendCommand((byte*) &visuaL3DSettransform, sizeof (DUCE.MILCMD_VISUAL3D_SETTRANSFORM));
      }
    }

    internal static class CompositionTarget
    {
      [SecurityCritical]
      internal static unsafe void HwndInitialize(
        DUCE.ResourceHandle hCompositionTarget,
        IntPtr hWnd,
        int nWidth,
        int nHeight,
        bool softwareOnly,
        int dpiAwarenessContext,
        DpiScale dpiScale,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_HWNDTARGET_CREATE hwndtargetCreate;
        hwndtargetCreate.Type = MILCMD.MilCmdHwndTargetCreate;
        hwndtargetCreate.Handle = hCompositionTarget;
        UIntPtr num = new UIntPtr(hWnd.ToPointer());
        hwndtargetCreate.hwnd = (ulong) num;
        hwndtargetCreate.width = (uint) nWidth;
        hwndtargetCreate.height = (uint) nHeight;
        hwndtargetCreate.clearColor.b = 0.0f;
        hwndtargetCreate.clearColor.r = 0.0f;
        hwndtargetCreate.clearColor.g = 0.0f;
        hwndtargetCreate.clearColor.a = 1f;
        hwndtargetCreate.flags = 12U;
        if (softwareOnly)
          hwndtargetCreate.flags |= 1U;
        bool? monitorDisplayClipping = CoreCompatibilityPreferences.EnableMultiMonitorDisplayClipping;
        if (monitorDisplayClipping.HasValue)
        {
          hwndtargetCreate.flags |= 32768U;
          if (!monitorDisplayClipping.Value)
            hwndtargetCreate.flags |= 16384U;
        }
        hwndtargetCreate.hBitmap = DUCE.ResourceHandle.Null;
        hwndtargetCreate.stride = 0U;
        hwndtargetCreate.ePixelFormat = 0U;
        hwndtargetCreate.hSection = 0UL;
        hwndtargetCreate.masterDevice = 0UL;
        hwndtargetCreate.DpiAwarenessContext = dpiAwarenessContext;
        hwndtargetCreate.DpiX = dpiScale.DpiScaleX;
        hwndtargetCreate.DpiY = dpiScale.DpiScaleY;
        channel.SendCommand((byte*) &hwndtargetCreate, sizeof (DUCE.MILCMD_HWNDTARGET_CREATE), false);
      }

      [SecuritySafeCritical]
      internal static unsafe void ProcessDpiChanged(
        DUCE.ResourceHandle hCompositionTarget,
        DpiScale dpiScale,
        bool afterParent,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_HWNDTARGET_DPICHANGED hwndtargetDpichanged;
        hwndtargetDpichanged.Type = MILCMD.MilCmdHwndTargetDpiChanged;
        hwndtargetDpichanged.Handle = hCompositionTarget;
        hwndtargetDpichanged.DpiX = dpiScale.DpiScaleX;
        hwndtargetDpichanged.DpiY = dpiScale.DpiScaleY;
        hwndtargetDpichanged.AfterParent = afterParent ? 1U : 0U;
        channel.SendCommand((byte*) &hwndtargetDpichanged, sizeof (DUCE.MILCMD_HWNDTARGET_DPICHANGED), false);
      }

      [SecurityCritical]
      internal static unsafe void PrintInitialize(
        DUCE.ResourceHandle hCompositionTarget,
        IntPtr pRenderTarget,
        int nWidth,
        int nHeight,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_GENERICTARGET_CREATE generictargetCreate;
        generictargetCreate.Type = MILCMD.MilCmdGenericTargetCreate;
        generictargetCreate.Handle = hCompositionTarget;
        generictargetCreate.hwnd = 0UL;
        generictargetCreate.pRenderTarget = (ulong) (long) pRenderTarget;
        generictargetCreate.width = (uint) nWidth;
        generictargetCreate.height = (uint) nHeight;
        generictargetCreate.dummy = 0U;
        channel.SendCommand((byte*) &generictargetCreate, sizeof (DUCE.MILCMD_GENERICTARGET_CREATE), false);
      }

      [SecurityTreatAsSafe]
      [SecurityCritical]
      internal static unsafe void SetClearColor(
        DUCE.ResourceHandle hCompositionTarget,
        Color color,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_TARGET_SETCLEARCOLOR targetSetclearcolor;
        targetSetclearcolor.Type = MILCMD.MilCmdTargetSetClearColor;
        targetSetclearcolor.Handle = hCompositionTarget;
        targetSetclearcolor.clearColor.b = color.ScB;
        targetSetclearcolor.clearColor.r = color.ScR;
        targetSetclearcolor.clearColor.g = color.ScG;
        targetSetclearcolor.clearColor.a = color.ScA;
        channel.SendCommand((byte*) &targetSetclearcolor, sizeof (DUCE.MILCMD_TARGET_SETCLEARCOLOR));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetRenderingMode(
        DUCE.ResourceHandle hCompositionTarget,
        MILRTInitializationFlags nRenderingMode,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_TARGET_SETFLAGS milcmdTargetSetflags;
        milcmdTargetSetflags.Type = MILCMD.MilCmdTargetSetFlags;
        milcmdTargetSetflags.Handle = hCompositionTarget;
        milcmdTargetSetflags.flags = (uint) nRenderingMode;
        channel.SendCommand((byte*) &milcmdTargetSetflags, sizeof (DUCE.MILCMD_TARGET_SETFLAGS));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void SetRoot(
        DUCE.ResourceHandle hCompositionTarget,
        DUCE.ResourceHandle hRoot,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_TARGET_SETROOT milcmdTargetSetroot;
        milcmdTargetSetroot.Type = MILCMD.MilCmdTargetSetRoot;
        milcmdTargetSetroot.Handle = hCompositionTarget;
        milcmdTargetSetroot.hRoot = hRoot;
        channel.SendCommand((byte*) &milcmdTargetSetroot, sizeof (DUCE.MILCMD_TARGET_SETROOT));
      }

      [SecurityCritical]
      internal static unsafe void UpdateWindowSettings(
        DUCE.ResourceHandle hCompositionTarget,
        MS.Win32.NativeMethods.RECT windowRect,
        Color colorKey,
        float constantAlpha,
        MILWindowLayerType windowLayerType,
        MILTransparencyFlags transparencyMode,
        bool isChild,
        bool isRTL,
        bool renderingEnabled,
        int disableCookie,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_TARGET_UPDATEWINDOWSETTINGS updatewindowsettings;
        updatewindowsettings.Type = MILCMD.MilCmdTargetUpdateWindowSettings;
        updatewindowsettings.Handle = hCompositionTarget;
        updatewindowsettings.renderingEnabled = renderingEnabled ? 1U : 0U;
        updatewindowsettings.disableCookie = (uint) disableCookie;
        updatewindowsettings.windowRect = windowRect;
        updatewindowsettings.colorKey.b = colorKey.ScB;
        updatewindowsettings.colorKey.r = colorKey.ScR;
        updatewindowsettings.colorKey.g = colorKey.ScG;
        updatewindowsettings.colorKey.a = colorKey.ScA;
        updatewindowsettings.constantAlpha = constantAlpha;
        updatewindowsettings.transparencyMode = transparencyMode;
        updatewindowsettings.windowLayerType = windowLayerType;
        updatewindowsettings.isChild = isChild ? 1U : 0U;
        updatewindowsettings.isRTL = isRTL ? 1U : 0U;
        channel.SendCommand((byte*) &updatewindowsettings, sizeof (DUCE.MILCMD_TARGET_UPDATEWINDOWSETTINGS));
      }

      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void Invalidate(
        DUCE.ResourceHandle hCompositionTarget,
        ref MS.Win32.NativeMethods.RECT pRect,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_TARGET_INVALIDATE targetInvalidate;
        targetInvalidate.Type = MILCMD.MilCmdTargetInvalidate;
        targetInvalidate.Handle = hCompositionTarget;
        targetInvalidate.rc = pRect;
        channel.SendCommand((byte*) &targetInvalidate, sizeof (DUCE.MILCMD_TARGET_INVALIDATE), false);
        channel.CloseBatch();
        channel.Commit();
      }
    }

    internal static class ETWEvent
    {
      [SecurityCritical]
      [SecurityTreatAsSafe]
      internal static unsafe void RaiseEvent(
        DUCE.ResourceHandle hEtwEvent,
        uint id,
        DUCE.Channel channel)
      {
        DUCE.MILCMD_ETWEVENTRESOURCE etweventresource;
        etweventresource.Type = MILCMD.MilCmdEtwEventResource;
        etweventresource.Handle = hEtwEvent;
        etweventresource.id = id;
        channel.SendCommand((byte*) &etweventresource, sizeof (DUCE.MILCMD_ETWEVENTRESOURCE));
      }
    }

    internal interface IResource
    {
      DUCE.ResourceHandle AddRefOnChannel(DUCE.Channel channel);

      int GetChannelCount();

      DUCE.Channel GetChannel(int index);

      void ReleaseOnChannel(DUCE.Channel channel);

      DUCE.ResourceHandle GetHandle(DUCE.Channel channel);

      DUCE.ResourceHandle Get3DHandle(DUCE.Channel channel);

      void RemoveChildFromParent(DUCE.IResource parent, DUCE.Channel channel);
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PARTITION_REGISTERFORNOTIFICATIONS
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal uint Enable;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_CHANNEL_REQUESTTIER
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal uint ReturnCommonMinimum;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PARTITION_SETVBLANKSYNCMODE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal uint Enable;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PARTITION_NOTIFYPRESENT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal ulong FrameTime;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_D3DIMAGE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ulong pInteropDeviceBitmap;
      [FieldOffset(16)]
      internal ulong pSoftwareBitmap;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_D3DIMAGE_PRESENT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ulong hEvent;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_BITMAP_INVALIDATE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint UseDirtyRect;
      [FieldOffset(12)]
      internal MS.Win32.NativeMethods.RECT DirtyRect;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DOUBLERESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_COLORRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_POINTRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Point Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_RECTRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Rect Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SIZERESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Size Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_MATRIXRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilMatrix3x2D Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_POINT3DRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilPoint3F Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VECTOR3DRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilPoint3F Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_QUATERNIONRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilQuaternionF Value;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_RENDERDATA
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint cbData;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_ETWEVENTRESOURCE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint id;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETOFFSET
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double offsetX;
      [FieldOffset(16)]
      internal double offsetY;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETTRANSFORM
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hTransform;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETEFFECT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hEffect;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETCACHEMODE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hCacheMode;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETCLIP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hClip;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETALPHA
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double alpha;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETRENDEROPTIONS
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilRenderOptions renderOptions;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETCONTENT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hContent;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETALPHAMASK
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hAlphaMask;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_REMOVEALLCHILDREN
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_REMOVECHILD
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hChild;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_INSERTCHILDAT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hChild;
      [FieldOffset(12)]
      internal uint index;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETGUIDELINECOLLECTION
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ushort countX;
      [FieldOffset(12)]
      internal ushort countY;
      [FieldOffset(15)]
      private byte BYTEPacking0;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL_SETSCROLLABLEAREACLIP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Rect Clip;
      [FieldOffset(40)]
      internal uint IsEnabled;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VIEWPORT3DVISUAL_SETCAMERA
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hCamera;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VIEWPORT3DVISUAL_SETVIEWPORT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Rect Viewport;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VIEWPORT3DVISUAL_SET3DCHILD
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hChild;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL3D_SETCONTENT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hContent;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL3D_SETTRANSFORM
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hTransform;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL3D_REMOVEALLCHILDREN
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL3D_REMOVECHILD
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hChild;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUAL3D_INSERTCHILDAT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hChild;
      [FieldOffset(12)]
      internal uint index;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_HWNDTARGET_CREATE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ulong hwnd;
      [FieldOffset(16)]
      internal ulong hSection;
      [FieldOffset(24)]
      internal ulong masterDevice;
      [FieldOffset(32)]
      internal uint width;
      [FieldOffset(36)]
      internal uint height;
      [FieldOffset(40)]
      internal MilColorF clearColor;
      [FieldOffset(56)]
      internal uint flags;
      [FieldOffset(60)]
      internal DUCE.ResourceHandle hBitmap;
      [FieldOffset(64)]
      internal uint stride;
      [FieldOffset(68)]
      internal uint ePixelFormat;
      [FieldOffset(72)]
      internal int DpiAwarenessContext;
      [FieldOffset(76)]
      internal double DpiX;
      [FieldOffset(84)]
      internal double DpiY;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TARGET_UPDATEWINDOWSETTINGS
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MS.Win32.NativeMethods.RECT windowRect;
      [FieldOffset(24)]
      internal MILWindowLayerType windowLayerType;
      [FieldOffset(28)]
      internal MILTransparencyFlags transparencyMode;
      [FieldOffset(32)]
      internal float constantAlpha;
      [FieldOffset(36)]
      internal uint isChild;
      [FieldOffset(40)]
      internal uint isRTL;
      [FieldOffset(44)]
      internal uint renderingEnabled;
      [FieldOffset(48)]
      internal MilColorF colorKey;
      [FieldOffset(64)]
      internal uint disableCookie;
      [FieldOffset(68)]
      internal uint gdiBlt;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_GENERICTARGET_CREATE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ulong hwnd;
      [FieldOffset(16)]
      internal ulong pRenderTarget;
      [FieldOffset(24)]
      internal uint width;
      [FieldOffset(28)]
      internal uint height;
      [FieldOffset(32)]
      internal uint dummy;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TARGET_SETROOT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hRoot;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TARGET_SETCLEARCOLOR
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF clearColor;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TARGET_INVALIDATE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MS.Win32.NativeMethods.RECT rc;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TARGET_SETFLAGS
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint flags;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_HWNDTARGET_DPICHANGED
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double DpiX;
      [FieldOffset(16)]
      internal double DpiY;
      [FieldOffset(24)]
      internal uint AfterParent;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_GLYPHRUN_CREATE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ulong pIDWriteFont;
      [FieldOffset(16)]
      internal ushort GlyphRunFlags;
      [FieldOffset(20)]
      internal MilPoint2F Origin;
      [FieldOffset(28)]
      internal float MuSize;
      [FieldOffset(32)]
      internal Rect ManagedBounds;
      [FieldOffset(64)]
      internal ushort GlyphCount;
      [FieldOffset(68)]
      internal ushort BidiLevel;
      [FieldOffset(72)]
      internal ushort DWriteTextMeasuringMethod;
      [FieldOffset(75)]
      private byte BYTEPacking0;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DOUBLEBUFFEREDBITMAP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ulong SwDoubleBufferedBitmap;
      [FieldOffset(16)]
      internal uint UseBackBuffer;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DOUBLEBUFFEREDBITMAP_COPYFORWARD
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ulong CopyCompletedEvent;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PARTITION_NOTIFYPOLICYCHANGEFORNONINTERACTIVEMODE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal uint ShouldRenderEvenWhenNoDisplayDevicesAreAvailable;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_AXISANGLEROTATION3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double angle;
      [FieldOffset(16)]
      internal MilPoint3F axis;
      [FieldOffset(28)]
      internal DUCE.ResourceHandle hAxisAnimations;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hAngleAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_QUATERNIONROTATION3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilQuaternionF quaternion;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle hQuaternionAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PERSPECTIVECAMERA
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double nearPlaneDistance;
      [FieldOffset(16)]
      internal double farPlaneDistance;
      [FieldOffset(24)]
      internal double fieldOfView;
      [FieldOffset(32)]
      internal MilPoint3F position;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(48)]
      internal MilPoint3F lookDirection;
      [FieldOffset(60)]
      internal DUCE.ResourceHandle hNearPlaneDistanceAnimations;
      [FieldOffset(64)]
      internal MilPoint3F upDirection;
      [FieldOffset(76)]
      internal DUCE.ResourceHandle hFarPlaneDistanceAnimations;
      [FieldOffset(80)]
      internal DUCE.ResourceHandle hPositionAnimations;
      [FieldOffset(84)]
      internal DUCE.ResourceHandle hLookDirectionAnimations;
      [FieldOffset(88)]
      internal DUCE.ResourceHandle hUpDirectionAnimations;
      [FieldOffset(92)]
      internal DUCE.ResourceHandle hFieldOfViewAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_ORTHOGRAPHICCAMERA
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double nearPlaneDistance;
      [FieldOffset(16)]
      internal double farPlaneDistance;
      [FieldOffset(24)]
      internal double width;
      [FieldOffset(32)]
      internal MilPoint3F position;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(48)]
      internal MilPoint3F lookDirection;
      [FieldOffset(60)]
      internal DUCE.ResourceHandle hNearPlaneDistanceAnimations;
      [FieldOffset(64)]
      internal MilPoint3F upDirection;
      [FieldOffset(76)]
      internal DUCE.ResourceHandle hFarPlaneDistanceAnimations;
      [FieldOffset(80)]
      internal DUCE.ResourceHandle hPositionAnimations;
      [FieldOffset(84)]
      internal DUCE.ResourceHandle hLookDirectionAnimations;
      [FieldOffset(88)]
      internal DUCE.ResourceHandle hUpDirectionAnimations;
      [FieldOffset(92)]
      internal DUCE.ResourceHandle hWidthAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_MATRIXCAMERA
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal D3DMATRIX viewMatrix;
      [FieldOffset(72)]
      internal D3DMATRIX projectionMatrix;
      [FieldOffset(136)]
      internal DUCE.ResourceHandle htransform;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_MODEL3DGROUP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(12)]
      internal uint ChildrenSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_AMBIENTLIGHT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF color;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(28)]
      internal DUCE.ResourceHandle hColorAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DIRECTIONALLIGHT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF color;
      [FieldOffset(24)]
      internal MilPoint3F direction;
      [FieldOffset(36)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hColorAnimations;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hDirectionAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_POINTLIGHT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF color;
      [FieldOffset(24)]
      internal double range;
      [FieldOffset(32)]
      internal double constantAttenuation;
      [FieldOffset(40)]
      internal double linearAttenuation;
      [FieldOffset(48)]
      internal double quadraticAttenuation;
      [FieldOffset(56)]
      internal MilPoint3F position;
      [FieldOffset(68)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(72)]
      internal DUCE.ResourceHandle hColorAnimations;
      [FieldOffset(76)]
      internal DUCE.ResourceHandle hPositionAnimations;
      [FieldOffset(80)]
      internal DUCE.ResourceHandle hRangeAnimations;
      [FieldOffset(84)]
      internal DUCE.ResourceHandle hConstantAttenuationAnimations;
      [FieldOffset(88)]
      internal DUCE.ResourceHandle hLinearAttenuationAnimations;
      [FieldOffset(92)]
      internal DUCE.ResourceHandle hQuadraticAttenuationAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SPOTLIGHT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF color;
      [FieldOffset(24)]
      internal double range;
      [FieldOffset(32)]
      internal double constantAttenuation;
      [FieldOffset(40)]
      internal double linearAttenuation;
      [FieldOffset(48)]
      internal double quadraticAttenuation;
      [FieldOffset(56)]
      internal double outerConeAngle;
      [FieldOffset(64)]
      internal double innerConeAngle;
      [FieldOffset(72)]
      internal MilPoint3F position;
      [FieldOffset(84)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(88)]
      internal MilPoint3F direction;
      [FieldOffset(100)]
      internal DUCE.ResourceHandle hColorAnimations;
      [FieldOffset(104)]
      internal DUCE.ResourceHandle hPositionAnimations;
      [FieldOffset(108)]
      internal DUCE.ResourceHandle hRangeAnimations;
      [FieldOffset(112)]
      internal DUCE.ResourceHandle hConstantAttenuationAnimations;
      [FieldOffset(116)]
      internal DUCE.ResourceHandle hLinearAttenuationAnimations;
      [FieldOffset(120)]
      internal DUCE.ResourceHandle hQuadraticAttenuationAnimations;
      [FieldOffset(124)]
      internal DUCE.ResourceHandle hDirectionAnimations;
      [FieldOffset(128)]
      internal DUCE.ResourceHandle hOuterConeAngleAnimations;
      [FieldOffset(132)]
      internal DUCE.ResourceHandle hInnerConeAngleAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_GEOMETRYMODEL3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle htransform;
      [FieldOffset(12)]
      internal DUCE.ResourceHandle hgeometry;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hmaterial;
      [FieldOffset(20)]
      internal DUCE.ResourceHandle hbackMaterial;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_MESHGEOMETRY3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint PositionsSize;
      [FieldOffset(12)]
      internal uint NormalsSize;
      [FieldOffset(16)]
      internal uint TextureCoordinatesSize;
      [FieldOffset(20)]
      internal uint TriangleIndicesSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_MATERIALGROUP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint ChildrenSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DIFFUSEMATERIAL
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF color;
      [FieldOffset(24)]
      internal MilColorF ambientColor;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hbrush;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SPECULARMATERIAL
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF color;
      [FieldOffset(24)]
      internal double specularPower;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hbrush;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_EMISSIVEMATERIAL
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilColorF color;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle hbrush;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TRANSFORM3DGROUP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint ChildrenSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TRANSLATETRANSFORM3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double offsetX;
      [FieldOffset(16)]
      internal double offsetY;
      [FieldOffset(24)]
      internal double offsetZ;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hOffsetXAnimations;
      [FieldOffset(36)]
      internal DUCE.ResourceHandle hOffsetYAnimations;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hOffsetZAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SCALETRANSFORM3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double scaleX;
      [FieldOffset(16)]
      internal double scaleY;
      [FieldOffset(24)]
      internal double scaleZ;
      [FieldOffset(32)]
      internal double centerX;
      [FieldOffset(40)]
      internal double centerY;
      [FieldOffset(48)]
      internal double centerZ;
      [FieldOffset(56)]
      internal DUCE.ResourceHandle hScaleXAnimations;
      [FieldOffset(60)]
      internal DUCE.ResourceHandle hScaleYAnimations;
      [FieldOffset(64)]
      internal DUCE.ResourceHandle hScaleZAnimations;
      [FieldOffset(68)]
      internal DUCE.ResourceHandle hCenterXAnimations;
      [FieldOffset(72)]
      internal DUCE.ResourceHandle hCenterYAnimations;
      [FieldOffset(76)]
      internal DUCE.ResourceHandle hCenterZAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_ROTATETRANSFORM3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double centerX;
      [FieldOffset(16)]
      internal double centerY;
      [FieldOffset(24)]
      internal double centerZ;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hCenterXAnimations;
      [FieldOffset(36)]
      internal DUCE.ResourceHandle hCenterYAnimations;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hCenterZAnimations;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hrotation;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_MATRIXTRANSFORM3D
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal D3DMATRIX matrix;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PIXELSHADER
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal ShaderRenderMode ShaderRenderMode;
      [FieldOffset(12)]
      internal uint PixelShaderBytecodeSize;
      [FieldOffset(16)]
      internal uint CompileSoftwareShader;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_IMPLICITINPUTBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(20)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle hRelativeTransform;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_BLUREFFECT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Radius;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hRadiusAnimations;
      [FieldOffset(20)]
      internal KernelType KernelType;
      [FieldOffset(24)]
      internal RenderingBias RenderingBias;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DROPSHADOWEFFECT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double ShadowDepth;
      [FieldOffset(16)]
      internal MilColorF Color;
      [FieldOffset(32)]
      internal double Direction;
      [FieldOffset(40)]
      internal double Opacity;
      [FieldOffset(48)]
      internal double BlurRadius;
      [FieldOffset(56)]
      internal DUCE.ResourceHandle hShadowDepthAnimations;
      [FieldOffset(60)]
      internal DUCE.ResourceHandle hColorAnimations;
      [FieldOffset(64)]
      internal DUCE.ResourceHandle hDirectionAnimations;
      [FieldOffset(68)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(72)]
      internal DUCE.ResourceHandle hBlurRadiusAnimations;
      [FieldOffset(76)]
      internal RenderingBias RenderingBias;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SHADEREFFECT
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double TopPadding;
      [FieldOffset(16)]
      internal double BottomPadding;
      [FieldOffset(24)]
      internal double LeftPadding;
      [FieldOffset(32)]
      internal double RightPadding;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hPixelShader;
      [FieldOffset(44)]
      internal int DdxUvDdyUvRegisterIndex;
      [FieldOffset(48)]
      internal uint ShaderConstantFloatRegistersSize;
      [FieldOffset(52)]
      internal uint DependencyPropertyFloatValuesSize;
      [FieldOffset(56)]
      internal uint ShaderConstantIntRegistersSize;
      [FieldOffset(60)]
      internal uint DependencyPropertyIntValuesSize;
      [FieldOffset(64)]
      internal uint ShaderConstantBoolRegistersSize;
      [FieldOffset(68)]
      internal uint DependencyPropertyBoolValuesSize;
      [FieldOffset(72)]
      internal uint ShaderSamplerRegistrationInfoSize;
      [FieldOffset(76)]
      internal uint DependencyPropertySamplerValuesSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DRAWINGIMAGE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hDrawing;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TRANSFORMGROUP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint ChildrenSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_TRANSLATETRANSFORM
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double X;
      [FieldOffset(16)]
      internal double Y;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle hXAnimations;
      [FieldOffset(28)]
      internal DUCE.ResourceHandle hYAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SCALETRANSFORM
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double ScaleX;
      [FieldOffset(16)]
      internal double ScaleY;
      [FieldOffset(24)]
      internal double CenterX;
      [FieldOffset(32)]
      internal double CenterY;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hScaleXAnimations;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hScaleYAnimations;
      [FieldOffset(48)]
      internal DUCE.ResourceHandle hCenterXAnimations;
      [FieldOffset(52)]
      internal DUCE.ResourceHandle hCenterYAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SKEWTRANSFORM
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double AngleX;
      [FieldOffset(16)]
      internal double AngleY;
      [FieldOffset(24)]
      internal double CenterX;
      [FieldOffset(32)]
      internal double CenterY;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hAngleXAnimations;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hAngleYAnimations;
      [FieldOffset(48)]
      internal DUCE.ResourceHandle hCenterXAnimations;
      [FieldOffset(52)]
      internal DUCE.ResourceHandle hCenterYAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_ROTATETRANSFORM
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Angle;
      [FieldOffset(16)]
      internal double CenterX;
      [FieldOffset(24)]
      internal double CenterY;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hAngleAnimations;
      [FieldOffset(36)]
      internal DUCE.ResourceHandle hCenterXAnimations;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hCenterYAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_MATRIXTRANSFORM
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal MilMatrix3x2D Matrix;
      [FieldOffset(56)]
      internal DUCE.ResourceHandle hMatrixAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_LINEGEOMETRY
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Point StartPoint;
      [FieldOffset(24)]
      internal Point EndPoint;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hStartPointAnimations;
      [FieldOffset(48)]
      internal DUCE.ResourceHandle hEndPointAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_RECTANGLEGEOMETRY
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double RadiusX;
      [FieldOffset(16)]
      internal double RadiusY;
      [FieldOffset(24)]
      internal Rect Rect;
      [FieldOffset(56)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(60)]
      internal DUCE.ResourceHandle hRadiusXAnimations;
      [FieldOffset(64)]
      internal DUCE.ResourceHandle hRadiusYAnimations;
      [FieldOffset(68)]
      internal DUCE.ResourceHandle hRectAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_ELLIPSEGEOMETRY
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double RadiusX;
      [FieldOffset(16)]
      internal double RadiusY;
      [FieldOffset(24)]
      internal Point Center;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hRadiusXAnimations;
      [FieldOffset(48)]
      internal DUCE.ResourceHandle hRadiusYAnimations;
      [FieldOffset(52)]
      internal DUCE.ResourceHandle hCenterAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_GEOMETRYGROUP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(12)]
      internal FillRule FillRule;
      [FieldOffset(16)]
      internal uint ChildrenSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_COMBINEDGEOMETRY
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(12)]
      internal GeometryCombineMode GeometryCombineMode;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hGeometry1;
      [FieldOffset(20)]
      internal DUCE.ResourceHandle hGeometry2;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PATHGEOMETRY
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(12)]
      internal FillRule FillRule;
      [FieldOffset(16)]
      internal uint FiguresSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_SOLIDCOLORBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal MilColorF Color;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(36)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hRelativeTransform;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hColorAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_LINEARGRADIENTBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal Point StartPoint;
      [FieldOffset(32)]
      internal Point EndPoint;
      [FieldOffset(48)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(52)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(56)]
      internal DUCE.ResourceHandle hRelativeTransform;
      [FieldOffset(60)]
      internal ColorInterpolationMode ColorInterpolationMode;
      [FieldOffset(64)]
      internal BrushMappingMode MappingMode;
      [FieldOffset(68)]
      internal GradientSpreadMethod SpreadMethod;
      [FieldOffset(72)]
      internal uint GradientStopsSize;
      [FieldOffset(76)]
      internal DUCE.ResourceHandle hStartPointAnimations;
      [FieldOffset(80)]
      internal DUCE.ResourceHandle hEndPointAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_RADIALGRADIENTBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal Point Center;
      [FieldOffset(32)]
      internal double RadiusX;
      [FieldOffset(40)]
      internal double RadiusY;
      [FieldOffset(48)]
      internal Point GradientOrigin;
      [FieldOffset(64)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(68)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(72)]
      internal DUCE.ResourceHandle hRelativeTransform;
      [FieldOffset(76)]
      internal ColorInterpolationMode ColorInterpolationMode;
      [FieldOffset(80)]
      internal BrushMappingMode MappingMode;
      [FieldOffset(84)]
      internal GradientSpreadMethod SpreadMethod;
      [FieldOffset(88)]
      internal uint GradientStopsSize;
      [FieldOffset(92)]
      internal DUCE.ResourceHandle hCenterAnimations;
      [FieldOffset(96)]
      internal DUCE.ResourceHandle hRadiusXAnimations;
      [FieldOffset(100)]
      internal DUCE.ResourceHandle hRadiusYAnimations;
      [FieldOffset(104)]
      internal DUCE.ResourceHandle hGradientOriginAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_IMAGEBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal Rect Viewport;
      [FieldOffset(48)]
      internal Rect Viewbox;
      [FieldOffset(80)]
      internal double CacheInvalidationThresholdMinimum;
      [FieldOffset(88)]
      internal double CacheInvalidationThresholdMaximum;
      [FieldOffset(96)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(100)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(104)]
      internal DUCE.ResourceHandle hRelativeTransform;
      [FieldOffset(108)]
      internal BrushMappingMode ViewportUnits;
      [FieldOffset(112)]
      internal BrushMappingMode ViewboxUnits;
      [FieldOffset(116)]
      internal DUCE.ResourceHandle hViewportAnimations;
      [FieldOffset(120)]
      internal DUCE.ResourceHandle hViewboxAnimations;
      [FieldOffset(124)]
      internal Stretch Stretch;
      [FieldOffset(128)]
      internal TileMode TileMode;
      [FieldOffset(132)]
      internal AlignmentX AlignmentX;
      [FieldOffset(136)]
      internal AlignmentY AlignmentY;
      [FieldOffset(140)]
      internal CachingHint CachingHint;
      [FieldOffset(144)]
      internal DUCE.ResourceHandle hImageSource;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DRAWINGBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal Rect Viewport;
      [FieldOffset(48)]
      internal Rect Viewbox;
      [FieldOffset(80)]
      internal double CacheInvalidationThresholdMinimum;
      [FieldOffset(88)]
      internal double CacheInvalidationThresholdMaximum;
      [FieldOffset(96)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(100)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(104)]
      internal DUCE.ResourceHandle hRelativeTransform;
      [FieldOffset(108)]
      internal BrushMappingMode ViewportUnits;
      [FieldOffset(112)]
      internal BrushMappingMode ViewboxUnits;
      [FieldOffset(116)]
      internal DUCE.ResourceHandle hViewportAnimations;
      [FieldOffset(120)]
      internal DUCE.ResourceHandle hViewboxAnimations;
      [FieldOffset(124)]
      internal Stretch Stretch;
      [FieldOffset(128)]
      internal TileMode TileMode;
      [FieldOffset(132)]
      internal AlignmentX AlignmentX;
      [FieldOffset(136)]
      internal AlignmentY AlignmentY;
      [FieldOffset(140)]
      internal CachingHint CachingHint;
      [FieldOffset(144)]
      internal DUCE.ResourceHandle hDrawing;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VISUALBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal Rect Viewport;
      [FieldOffset(48)]
      internal Rect Viewbox;
      [FieldOffset(80)]
      internal double CacheInvalidationThresholdMinimum;
      [FieldOffset(88)]
      internal double CacheInvalidationThresholdMaximum;
      [FieldOffset(96)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(100)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(104)]
      internal DUCE.ResourceHandle hRelativeTransform;
      [FieldOffset(108)]
      internal BrushMappingMode ViewportUnits;
      [FieldOffset(112)]
      internal BrushMappingMode ViewboxUnits;
      [FieldOffset(116)]
      internal DUCE.ResourceHandle hViewportAnimations;
      [FieldOffset(120)]
      internal DUCE.ResourceHandle hViewboxAnimations;
      [FieldOffset(124)]
      internal Stretch Stretch;
      [FieldOffset(128)]
      internal TileMode TileMode;
      [FieldOffset(132)]
      internal AlignmentX AlignmentX;
      [FieldOffset(136)]
      internal AlignmentY AlignmentY;
      [FieldOffset(140)]
      internal CachingHint CachingHint;
      [FieldOffset(144)]
      internal DUCE.ResourceHandle hVisual;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_BITMAPCACHEBRUSH
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(20)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle hRelativeTransform;
      [FieldOffset(28)]
      internal DUCE.ResourceHandle hBitmapCache;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hInternalTarget;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DASHSTYLE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Offset;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hOffsetAnimations;
      [FieldOffset(20)]
      internal uint DashesSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_PEN
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Thickness;
      [FieldOffset(16)]
      internal double MiterLimit;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle hBrush;
      [FieldOffset(28)]
      internal DUCE.ResourceHandle hThicknessAnimations;
      [FieldOffset(32)]
      internal PenLineCap StartLineCap;
      [FieldOffset(36)]
      internal PenLineCap EndLineCap;
      [FieldOffset(40)]
      internal PenLineCap DashCap;
      [FieldOffset(44)]
      internal PenLineJoin LineJoin;
      [FieldOffset(48)]
      internal DUCE.ResourceHandle hDashStyle;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_GEOMETRYDRAWING
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hBrush;
      [FieldOffset(12)]
      internal DUCE.ResourceHandle hPen;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hGeometry;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_GLYPHRUNDRAWING
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal DUCE.ResourceHandle hGlyphRun;
      [FieldOffset(12)]
      internal DUCE.ResourceHandle hForegroundBrush;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_IMAGEDRAWING
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Rect Rect;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hImageSource;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hRectAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_VIDEODRAWING
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal Rect Rect;
      [FieldOffset(40)]
      internal DUCE.ResourceHandle hPlayer;
      [FieldOffset(44)]
      internal DUCE.ResourceHandle hRectAnimations;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_DRAWINGGROUP
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double Opacity;
      [FieldOffset(16)]
      internal uint ChildrenSize;
      [FieldOffset(20)]
      internal DUCE.ResourceHandle hClipGeometry;
      [FieldOffset(24)]
      internal DUCE.ResourceHandle hOpacityAnimations;
      [FieldOffset(28)]
      internal DUCE.ResourceHandle hOpacityMask;
      [FieldOffset(32)]
      internal DUCE.ResourceHandle hTransform;
      [FieldOffset(36)]
      internal DUCE.ResourceHandle hGuidelineSet;
      [FieldOffset(40)]
      internal EdgeMode EdgeMode;
      [FieldOffset(44)]
      internal BitmapScalingMode bitmapScalingMode;
      [FieldOffset(48)]
      internal ClearTypeHint ClearTypeHint;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_GUIDELINESET
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal uint GuidelinesXSize;
      [FieldOffset(12)]
      internal uint GuidelinesYSize;
      [FieldOffset(16)]
      internal uint IsDynamic;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MILCMD_BITMAPCACHE
    {
      [FieldOffset(0)]
      internal MILCMD Type;
      [FieldOffset(4)]
      internal DUCE.ResourceHandle Handle;
      [FieldOffset(8)]
      internal double RenderAtScale;
      [FieldOffset(16)]
      internal DUCE.ResourceHandle hRenderAtScaleAnimations;
      [FieldOffset(20)]
      internal uint SnapsToDevicePixels;
      [FieldOffset(24)]
      internal uint EnableClearType;
    }

    internal enum ResourceType
    {
      TYPE_NULL,
      TYPE_MEDIAPLAYER,
      TYPE_ROTATION3D,
      TYPE_AXISANGLEROTATION3D,
      TYPE_QUATERNIONROTATION3D,
      TYPE_CAMERA,
      TYPE_PROJECTIONCAMERA,
      TYPE_PERSPECTIVECAMERA,
      TYPE_ORTHOGRAPHICCAMERA,
      TYPE_MATRIXCAMERA,
      TYPE_MODEL3D,
      TYPE_MODEL3DGROUP,
      TYPE_LIGHT,
      TYPE_AMBIENTLIGHT,
      TYPE_DIRECTIONALLIGHT,
      TYPE_POINTLIGHTBASE,
      TYPE_POINTLIGHT,
      TYPE_SPOTLIGHT,
      TYPE_GEOMETRYMODEL3D,
      TYPE_GEOMETRY3D,
      TYPE_MESHGEOMETRY3D,
      TYPE_MATERIAL,
      TYPE_MATERIALGROUP,
      TYPE_DIFFUSEMATERIAL,
      TYPE_SPECULARMATERIAL,
      TYPE_EMISSIVEMATERIAL,
      TYPE_TRANSFORM3D,
      TYPE_TRANSFORM3DGROUP,
      TYPE_AFFINETRANSFORM3D,
      TYPE_TRANSLATETRANSFORM3D,
      TYPE_SCALETRANSFORM3D,
      TYPE_ROTATETRANSFORM3D,
      TYPE_MATRIXTRANSFORM3D,
      TYPE_PIXELSHADER,
      TYPE_IMPLICITINPUTBRUSH,
      TYPE_EFFECT,
      TYPE_BLUREFFECT,
      TYPE_DROPSHADOWEFFECT,
      TYPE_SHADEREFFECT,
      TYPE_VISUAL,
      TYPE_VIEWPORT3DVISUAL,
      TYPE_VISUAL3D,
      TYPE_GLYPHRUN,
      TYPE_RENDERDATA,
      TYPE_DRAWINGCONTEXT,
      TYPE_RENDERTARGET,
      TYPE_HWNDRENDERTARGET,
      TYPE_GENERICRENDERTARGET,
      TYPE_ETWEVENTRESOURCE,
      TYPE_DOUBLERESOURCE,
      TYPE_COLORRESOURCE,
      TYPE_POINTRESOURCE,
      TYPE_RECTRESOURCE,
      TYPE_SIZERESOURCE,
      TYPE_MATRIXRESOURCE,
      TYPE_POINT3DRESOURCE,
      TYPE_VECTOR3DRESOURCE,
      TYPE_QUATERNIONRESOURCE,
      TYPE_IMAGESOURCE,
      TYPE_DRAWINGIMAGE,
      TYPE_TRANSFORM,
      TYPE_TRANSFORMGROUP,
      TYPE_TRANSLATETRANSFORM,
      TYPE_SCALETRANSFORM,
      TYPE_SKEWTRANSFORM,
      TYPE_ROTATETRANSFORM,
      TYPE_MATRIXTRANSFORM,
      TYPE_GEOMETRY,
      TYPE_LINEGEOMETRY,
      TYPE_RECTANGLEGEOMETRY,
      TYPE_ELLIPSEGEOMETRY,
      TYPE_GEOMETRYGROUP,
      TYPE_COMBINEDGEOMETRY,
      TYPE_PATHGEOMETRY,
      TYPE_BRUSH,
      TYPE_SOLIDCOLORBRUSH,
      TYPE_GRADIENTBRUSH,
      TYPE_LINEARGRADIENTBRUSH,
      TYPE_RADIALGRADIENTBRUSH,
      TYPE_TILEBRUSH,
      TYPE_IMAGEBRUSH,
      TYPE_DRAWINGBRUSH,
      TYPE_VISUALBRUSH,
      TYPE_BITMAPCACHEBRUSH,
      TYPE_DASHSTYLE,
      TYPE_PEN,
      TYPE_DRAWING,
      TYPE_GEOMETRYDRAWING,
      TYPE_GLYPHRUNDRAWING,
      TYPE_IMAGEDRAWING,
      TYPE_VIDEODRAWING,
      TYPE_DRAWINGGROUP,
      TYPE_GUIDELINESET,
      TYPE_CACHEMODE,
      TYPE_BITMAPCACHE,
      TYPE_BITMAPSOURCE,
      TYPE_DOUBLEBUFFEREDBITMAP,
      TYPE_D3DIMAGE,
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct MIL_GRADIENTSTOP
    {
      [FieldOffset(0)]
      internal double Position;
      [FieldOffset(8)]
      internal MilColorF Color;
    }
  }
}
