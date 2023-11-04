// Decompiled with JetBrains decompiler
// Type: System.Windows.Media.Effects.ShaderEffect
// Assembly: PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CD7B53CF-E517-42D9-B6D9-F989F6E5A5A3
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\PresentationCore\v4.0_4.0.0.0__31bf3856ad364e35\PresentationCore.dll

using Decompiled_System.Windows.Media.Composition;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Composition;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Decompiled_System.Windows.Media.Effects
{
  /// <summary>
  /// <see cref="T:System.Windows.Media.Effects.PixelShader" /> を使用して、カスタム ビットマップ効果を実現します。</summary>
  public abstract class ShaderEffect : Effect
  {
    private const SamplingMode _defaultSamplingMode = SamplingMode.Auto;
    private double _topPadding;
    private double _bottomPadding;
    private double _leftPadding;
    private double _rightPadding;
    private List<MilColorF?> _floatRegisters;
    private List<MilColorI?> _intRegisters;
    private List<bool?> _boolRegisters;
    private List<ShaderEffect.SamplerData?> _samplerData;
    private uint _floatCount;
    private uint _intCount;
    private uint _boolCount;
    private uint _samplerCount;
    private int _ddxUvDdyUvRegisterIndex = -1;
    private const int PS_2_0_FLOAT_REGISTER_LIMIT = 32;
    private const int PS_3_0_FLOAT_REGISTER_LIMIT = 224;
    private const int PS_3_0_INT_REGISTER_LIMIT = 16;
    private const int PS_3_0_BOOL_REGISTER_LIMIT = 16;
    private const int PS_2_0_SAMPLER_LIMIT = 4;
    private const int PS_3_0_SAMPLER_LIMIT = 8;
    /// <summary>
    /// <see cref="P:System.Windows.Media.Effects.ShaderEffect.PixelShader" /> 依存関係プロパティを識別します。</summary>
    protected static readonly DependencyProperty PixelShaderProperty = Animatable.RegisterProperty(nameof (PixelShader), typeof (PixelShader), typeof (ShaderEffect), (object) null, new PropertyChangedCallback(ShaderEffect.PixelShaderPropertyChanged), (ValidateValueCallback) null, false, (CoerceValueCallback) null);
    internal DUCE.MultiChannelResource _duceResource;

    internal override Rect GetRenderBounds(Rect contentBounds)
    {
      Point point1 = new Point();
      Point point2 = new Point();
      point1.X = contentBounds.TopLeft.X - this.PaddingLeft;
      point1.Y = contentBounds.TopLeft.Y - this.PaddingTop;
      point2.X = contentBounds.BottomRight.X + this.PaddingRight;
      point2.Y = contentBounds.BottomRight.Y + this.PaddingBottom;
      return new Rect(point1, point2);
    }

    /// <summary>効果の出力テクスチャが、上端に合わせたその入力テクスチャより大きいことを示す値を取得または設定します。</summary>
    /// <returns>効果の上端に沿った埋め込み。</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">指定された値が 0 未満です。</exception>
    protected double PaddingTop
    {
      get
      {
        this.ReadPreamble();
        return this._topPadding;
      }
      set
      {
        this.WritePreamble();
        this._topPadding = value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), (object) value, MS.Internal.PresentationCore.SR.Get("Effect_ShaderEffectPadding"));
        this.RegisterForAsyncUpdateResource();
        this.WritePostscript();
      }
    }

    /// <summary>効果の出力テクスチャが、下端に合わせたその入力テクスチャより大きいことを示す値を取得または設定します。</summary>
    /// <returns>効果の下端に沿った埋め込み。</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">指定された値が 0 未満です。</exception>
    protected double PaddingBottom
    {
      get
      {
        this.ReadPreamble();
        return this._bottomPadding;
      }
      set
      {
        this.WritePreamble();
        this._bottomPadding = value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), (object) value, MS.Internal.PresentationCore.SR.Get("Effect_ShaderEffectPadding"));
        this.RegisterForAsyncUpdateResource();
        this.WritePostscript();
      }
    }

    /// <summary>効果の出力テクスチャが、左端に合わせたその入力テクスチャより大きいことを示す値を取得または設定します。</summary>
    /// <returns>効果の左端に沿った埋め込み。</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">指定された値が 0 未満です。</exception>
    protected double PaddingLeft
    {
      get
      {
        this.ReadPreamble();
        return this._leftPadding;
      }
      set
      {
        this.WritePreamble();
        this._leftPadding = value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), (object) value, MS.Internal.PresentationCore.SR.Get("Effect_ShaderEffectPadding"));
        this.RegisterForAsyncUpdateResource();
        this.WritePostscript();
      }
    }

    /// <summary>効果の出力テクスチャが、右端に合わせたその入力テクスチャより大きいことを示す値を取得または設定します。</summary>
    /// <returns>効果の右端に沿った埋め込み。</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">指定された値が 0 未満です。</exception>
    protected double PaddingRight
    {
      get
      {
        this.ReadPreamble();
        return this._rightPadding;
      }
      set
      {
        this.WritePreamble();
        this._rightPadding = value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), (object) value, MS.Internal.PresentationCore.SR.Get("Effect_ShaderEffectPadding"));
        this.RegisterForAsyncUpdateResource();
        this.WritePostscript();
      }
    }

    /// <summary>画面空間に対するテクスチャ座標の偏導関数で使用するシェーダー レジスタを示す値を取得または設定します。</summary>
    /// <returns>偏導関数を格納するレジスタのインデックス。</returns>
    /// <exception cref="T:System.InvalidOperationException">
    /// <see cref="P:System.Windows.Media.Effects.ShaderEffect.DdxUvDdyUvRegisterIndex" /> プロパティを、複数回、または効果の初期処理終了後に設定しようとしました。</exception>
    protected int DdxUvDdyUvRegisterIndex
    {
      get
      {
        this.ReadPreamble();
        return this._ddxUvDdyUvRegisterIndex;
      }
      set
      {
        this.WritePreamble();
        this._ddxUvDdyUvRegisterIndex = value;
        this.WritePostscript();
      }
    }

    private void PixelShaderPropertyChangedHook(DependencyPropertyChangedEventArgs e)
    {
      PixelShader oldValue = (PixelShader) e.OldValue;
      if (oldValue != null)
        oldValue._shaderBytecodeChanged -= new EventHandler(this.OnPixelShaderBytecodeChanged);
      PixelShader newValue = (PixelShader) e.NewValue;
      if (newValue != null)
        newValue._shaderBytecodeChanged += new EventHandler(this.OnPixelShaderBytecodeChanged);
      this.OnPixelShaderBytecodeChanged((object) this.PixelShader, (EventArgs) null);
    }

    private void OnPixelShaderBytecodeChanged(object sender, EventArgs e)
    {
      PixelShader pixelShader = (PixelShader) sender;
      if (pixelShader != null && pixelShader.ShaderMajorVersion == (short) 2 && (pixelShader.ShaderMinorVersion == (short) 0 && this.UsesPS30OnlyRegisters()))
        throw new InvalidOperationException(MS.Internal.PresentationCore.SR.Get("Effect_20ShaderUsing30Registers"));
    }

    private bool UsesPS30OnlyRegisters()
    {
      if (this._intCount > 0U || this._intRegisters != null || (this._boolCount > 0U || this._boolRegisters != null))
        return true;
      if (this._floatRegisters != null)
      {
        for (int index = 32; index < this._floatRegisters.Count; ++index)
        {
          if (this._floatRegisters[index].HasValue)
            return true;
        }
      }
      if (this._samplerData != null)
      {
        for (int index = 4; index < this._samplerData.Count; ++index)
        {
          if (this._samplerData[index].HasValue)
            return true;
        }
      }
      return false;
    }

    /// <summary>指定した依存関係プロパティに対応するシェーダー定数またはシェーダー サンプラーの更新が必要な効果を通知します。</summary>
    /// <param name="dp">更新される依存関係プロパティ。</param>
    protected void UpdateShaderValue(DependencyProperty dp)
    {
      if (dp == null)
        return;
      this.WritePreamble();
      object obj = this.GetValue(dp);
      PropertyMetadata metadata = dp.GetMetadata((DependencyObject) this);
      if (metadata != null)
      {
        PropertyChangedCallback propertyChangedCallback = metadata.PropertyChangedCallback;
        if (propertyChangedCallback != null)
          propertyChangedCallback((DependencyObject) this, new DependencyPropertyChangedEventArgs(dp, obj, obj));
      }
      this.WritePostscript();
    }

    /// <summary>依存関係プロパティの値をピクセル シェーダーの float 型定数レジスタに関連付けます。</summary>
    /// <param name="floatRegisterIndex">依存関係プロパティに関連付けられるシェーダー レジスタのインデックス。</param>
    /// <returns>依存関係プロパティと、<see cref="T:System.Windows.PropertyChangedCallback" /> で指定されたシェーダー定数レジスタを関連付ける <paramref name="floatRegisterIndex" /> デリゲート。</returns>
    /// <exception cref="T:System.InvalidOperationException">依存関係プロパティの型が不明です。</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="floatRegisterIndex" /> が 32 以上、または <paramref name="floatRegisterIndex" /> が 0 未満です。</exception>
    protected static PropertyChangedCallback PixelShaderConstantCallback(
      int floatRegisterIndex)
    {
      return (PropertyChangedCallback) ((obj, args) =>
      {
        if (!(obj is ShaderEffect shaderEffect))
          return;
        shaderEffect.UpdateShaderConstant(args.Property, args.NewValue, floatRegisterIndex);
      });
    }

    /// <summary>依存関係プロパティの値をピクセル シェーダーのサンプラー レジスタに関連付けます。</summary>
    /// <param name="samplerRegisterIndex">依存関係プロパティに関連付けられるシェーダー サンプラーのインデックス。</param>
    /// <returns>依存関係プロパティと、<see cref="T:System.Windows.PropertyChangedCallback" /> で指定されたシェーダー サンプラー レジスタを関連付ける <paramref name="samplerRegisterIndex" /> デリゲート。</returns>
    protected static PropertyChangedCallback PixelShaderSamplerCallback(
      int samplerRegisterIndex)
    {
      return ShaderEffect.PixelShaderSamplerCallback(samplerRegisterIndex, SamplingMode.Auto);
    }

    /// <summary>依存関係プロパティの値をピクセル シェーダーのサンプラー レジスタおよび <see cref="T:System.Windows.Media.Effects.SamplingMode" /> に関連付けます。</summary>
    /// <param name="samplerRegisterIndex">依存関係プロパティに関連付けられるシェーダー サンプラーのインデックス。</param>
    /// <param name="samplingMode">シェーダー サンプラーの <see cref="T:System.Windows.Media.Effects.SamplingMode" />。</param>
    /// <returns>依存関係プロパティと、<see cref="T:System.Windows.PropertyChangedCallback" /> で指定されたシェーダー サンプラー レジスタを関連付ける <paramref name="samplerRegisterIndex" /> デリゲート。</returns>
    protected static PropertyChangedCallback PixelShaderSamplerCallback(
      int samplerRegisterIndex,
      SamplingMode samplingMode)
    {
      return (PropertyChangedCallback) ((obj, args) =>
      {
        if (!(obj is ShaderEffect shaderEffect) || !args.IsAValueChange)
          return;
        shaderEffect.UpdateShaderSampler(args.Property, args.NewValue, samplerRegisterIndex, samplingMode);
      });
    }

    /// <summary>依存関係プロパティをシェーダー サンプラー レジスタに関連付けます。</summary>
    /// <param name="dpName">依存関係プロパティの名前。</param>
    /// <param name="ownerType">依存関係プロパティを持つ効果の種類。</param>
    /// <param name="samplerRegisterIndex">依存関係プロパティに関連付けられるシェーダー サンプラーのインデックス。</param>
    /// <returns>
    /// <paramref name="samplerRegisterIndex" /> で指定されるシェーダー サンプラーに関連付けられた依存関係プロパティ。</returns>
    protected static DependencyProperty RegisterPixelShaderSamplerProperty(
      string dpName,
      System.Type ownerType,
      int samplerRegisterIndex)
    {
      return ShaderEffect.RegisterPixelShaderSamplerProperty(dpName, ownerType, samplerRegisterIndex, SamplingMode.Auto);
    }

    /// <summary>依存関係プロパティをシェーダー サンプラ レジスタおよび <see cref="T:System.Windows.Media.Effects.SamplingMode" /> に関連付けます。</summary>
    /// <param name="dpName">依存関係プロパティの名前。</param>
    /// <param name="ownerType">依存関係プロパティを持つ効果の種類。</param>
    /// <param name="samplerRegisterIndex">依存関係プロパティに関連付けられるシェーダー サンプラーのインデックス。</param>
    /// <param name="samplingMode">シェーダー サンプラーの <see cref="T:System.Windows.Media.Effects.SamplingMode" />。</param>
    /// <returns>
    /// <paramref name="samplerRegisterIndex" /> で指定されるシェーダー サンプラーに関連付けられた依存関係プロパティ。</returns>
    protected static DependencyProperty RegisterPixelShaderSamplerProperty(
      string dpName,
      System.Type ownerType,
      int samplerRegisterIndex,
      SamplingMode samplingMode)
    {
      return DependencyProperty.Register(dpName, typeof (Brush), ownerType, (PropertyMetadata) new UIPropertyMetadata((object) Effect.ImplicitInput, ShaderEffect.PixelShaderSamplerCallback(samplerRegisterIndex, samplingMode)));
    }

    private void UpdateShaderConstant(DependencyProperty dp, object newValue, int registerIndex)
    {
      this.WritePreamble();
      System.Type shaderConstantType = ShaderEffect.DetermineShaderConstantType(dp.PropertyType, this.PixelShader);
      if (shaderConstantType == (System.Type) null)
        throw new InvalidOperationException(MS.Internal.PresentationCore.SR.Get("Effect_ShaderConstantType", (object) dp.PropertyType.Name));
      int maxIndex = 32;
      string id = "Effect_Shader20ConstantRegisterLimit";
      if (this.PixelShader != null && this.PixelShader.ShaderMajorVersion >= (short) 3)
      {
        if (shaderConstantType == typeof (float))
        {
          maxIndex = 224;
          id = "Effect_Shader30FloatConstantRegisterLimit";
        }
        else if (shaderConstantType == typeof (int))
        {
          maxIndex = 16;
          id = "Effect_Shader30IntConstantRegisterLimit";
        }
        else if (shaderConstantType == typeof (bool))
        {
          maxIndex = 16;
          id = "Effect_Shader30BoolConstantRegisterLimit";
        }
      }
      if (registerIndex >= maxIndex || registerIndex < 0)
        throw new ArgumentException(MS.Internal.PresentationCore.SR.Get(id), nameof (dp));
      if (shaderConstantType == typeof (float))
      {
        MilColorF newVal;
        ShaderEffect.ConvertValueToMilColorF(newValue, out newVal);
        ShaderEffect.StashInPosition<MilColorF>(ref this._floatRegisters, registerIndex, newVal, maxIndex, ref this._floatCount);
      }
      else if (shaderConstantType == typeof (int))
      {
        MilColorI newVal;
        ShaderEffect.ConvertValueToMilColorI(newValue, out newVal);
        ShaderEffect.StashInPosition<MilColorI>(ref this._intRegisters, registerIndex, newVal, maxIndex, ref this._intCount);
      }
      else if (shaderConstantType == typeof (bool))
        ShaderEffect.StashInPosition<bool>(ref this._boolRegisters, registerIndex, (bool) newValue, maxIndex, ref this._boolCount);
      this.PropertyChanged(dp);
      this.WritePostscript();
    }

    private void UpdateShaderSampler(
      DependencyProperty dp,
      object newValue,
      int registerIndex,
      SamplingMode samplingMode)
    {
      this.WritePreamble();
      if (newValue != null && !typeof (VisualBrush).IsInstanceOfType(newValue) && (!typeof (BitmapCacheBrush).IsInstanceOfType(newValue) && !typeof (ImplicitInputBrush).IsInstanceOfType(newValue)) && !typeof (ImageBrush).IsInstanceOfType(newValue))
        throw new ArgumentException(MS.Internal.PresentationCore.SR.Get("Effect_ShaderSamplerType"), nameof (dp));
      int maxIndex = 4;
      string id = "Effect_Shader20SamplerRegisterLimit";
      if (this.PixelShader != null && this.PixelShader.ShaderMajorVersion >= (short) 3)
      {
        maxIndex = 8;
        id = "Effect_Shader30SamplerRegisterLimit";
      }
      if (registerIndex >= maxIndex || registerIndex < 0)
        throw new ArgumentException(MS.Internal.PresentationCore.SR.Get(id));
      ShaderEffect.SamplerData newSampler = new ShaderEffect.SamplerData()
      {
        _brush = (Brush) newValue,
        _samplingMode = samplingMode
      };
      this.StashSamplerDataInPosition(registerIndex, newSampler, maxIndex);
      this.PropertyChanged(dp);
      this.WritePostscript();
    }

    private static void StashInPosition<T>(
      ref List<T?> list,
      int position,
      T value,
      int maxIndex,
      ref uint count)
      where T : struct
    {
      if (list == null)
        list = new List<T?>(maxIndex);
      if (list.Count <= position)
      {
        int num = position - list.Count + 1;
        for (int index = 0; index < num; ++index)
          list.Add(new T?());
      }
      if (!list[position].HasValue)
        ++count;
      list[position] = new T?(value);
    }

    private void StashSamplerDataInPosition(
      int position,
      ShaderEffect.SamplerData newSampler,
      int maxIndex)
    {
      if (this._samplerData == null)
        this._samplerData = new List<ShaderEffect.SamplerData?>(maxIndex);
      if (this._samplerData.Count <= position)
      {
        int num = position - this._samplerData.Count + 1;
        for (int index = 0; index < num; ++index)
          this._samplerData.Add(new ShaderEffect.SamplerData?());
      }
      if (!this._samplerData[position].HasValue)
        ++this._samplerCount;
      if (this.Dispatcher != null)
      {
        ShaderEffect.SamplerData? nullable = this._samplerData[position];
        Brush brush1 = (Brush) null;
        if (nullable.HasValue)
          brush1 = nullable.Value._brush;
        Brush brush2 = newSampler._brush;
        DUCE.IResource resource = (DUCE.IResource) this;
        using (CompositionEngineLock.Acquire())
        {
          int channelCount = resource.GetChannelCount();
          for (int index = 0; index < channelCount; ++index)
          {
            DUCE.Channel channel = resource.GetChannel(index);
            this.ReleaseResource((DUCE.IResource) brush1, channel);
            this.AddRefResource((DUCE.IResource) brush2, channel);
          }
        }
      }
      this._samplerData[position] = new ShaderEffect.SamplerData?(newSampler);
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    private unsafe void ManualUpdateResource(DUCE.Channel channel, bool skipOnChannelCheck)
    {
      if (!skipOnChannelCheck && !this._duceResource.IsOnChannel(channel))
        return;
      if (this.PixelShader == null)
        throw new InvalidOperationException(MS.Internal.PresentationCore.SR.Get("Effect_ShaderPixelShaderSet"));
      DUCE.MILCMD_SHADEREFFECT milcmdShadereffect;
      milcmdShadereffect.Type = MILCMD.MilCmdShaderEffect;
      milcmdShadereffect.Handle = this._duceResource.GetHandle(channel);
      milcmdShadereffect.TopPadding = this._topPadding;
      milcmdShadereffect.BottomPadding = this._bottomPadding;
      milcmdShadereffect.LeftPadding = this._leftPadding;
      milcmdShadereffect.RightPadding = this._rightPadding;
      milcmdShadereffect.DdxUvDdyUvRegisterIndex = this.DdxUvDdyUvRegisterIndex;
      milcmdShadereffect.hPixelShader = ((DUCE.IResource) this.PixelShader).GetHandle(channel);
      milcmdShadereffect.ShaderConstantFloatRegistersSize = checked (2U * this._floatCount);
      milcmdShadereffect.DependencyPropertyFloatValuesSize = checked (16U * this._floatCount);
      milcmdShadereffect.ShaderConstantIntRegistersSize = checked (2U * this._intCount);
      milcmdShadereffect.DependencyPropertyIntValuesSize = checked (16U * this._intCount);
      milcmdShadereffect.ShaderConstantBoolRegistersSize = checked (2U * this._boolCount);
      milcmdShadereffect.DependencyPropertyBoolValuesSize = checked (4U * this._boolCount);
      milcmdShadereffect.ShaderSamplerRegistrationInfoSize = checked (8U * this._samplerCount);
      milcmdShadereffect.DependencyPropertySamplerValuesSize = checked ((uint) ((long) sizeof (DUCE.ResourceHandle) * (long) this._samplerCount));
      channel.BeginCommand((byte*) &milcmdShadereffect, sizeof (DUCE.MILCMD_SHADEREFFECT), checked ((int) (milcmdShadereffect.ShaderConstantFloatRegistersSize + milcmdShadereffect.DependencyPropertyFloatValuesSize + milcmdShadereffect.ShaderConstantIntRegistersSize + milcmdShadereffect.DependencyPropertyIntValuesSize + milcmdShadereffect.ShaderConstantBoolRegistersSize + milcmdShadereffect.DependencyPropertyBoolValuesSize + milcmdShadereffect.ShaderSamplerRegistrationInfoSize + milcmdShadereffect.DependencyPropertySamplerValuesSize)));
      this.AppendRegisters<MilColorF>(channel, this._floatRegisters);
      if (this._floatRegisters != null)
      {
        int index = 0;
        while (index < this._floatRegisters.Count)
        {
          MilColorF? floatRegister = this._floatRegisters[index];
          if (floatRegister.HasValue)
          {
            MilColorF milColorF = floatRegister.Value;
            channel.AppendCommandData((byte*) &milColorF, sizeof (MilColorF));
          }
          checked { ++index; }
        }
      }
      this.AppendRegisters<MilColorI>(channel, this._intRegisters);
      if (this._intRegisters != null)
      {
        int index = 0;
        while (index < this._intRegisters.Count)
        {
          MilColorI? intRegister = this._intRegisters[index];
          if (intRegister.HasValue)
          {
            MilColorI milColorI = intRegister.Value;
            channel.AppendCommandData((byte*) &milColorI, sizeof (MilColorI));
          }
          checked { ++index; }
        }
      }
      this.AppendRegisters<bool>(channel, this._boolRegisters);
      if (this._boolRegisters != null)
      {
        int index = 0;
        while (index < this._boolRegisters.Count)
        {
          bool? boolRegister = this._boolRegisters[index];
          if (boolRegister.HasValue)
          {
            int num = boolRegister.Value ? 1 : 0;
            channel.AppendCommandData((byte*) &num, 4);
          }
          checked { ++index; }
        }
      }
      if (this._samplerCount > 0U)
      {
        int count = this._samplerData.Count;
        int index = 0;
        while (index < count)
        {
          ShaderEffect.SamplerData? nullable = this._samplerData[index];
          if (nullable.HasValue)
          {
            ShaderEffect.SamplerData samplerData = nullable.Value;
            channel.AppendCommandData((byte*) &index, 4);
            int samplingMode = (int) samplerData._samplingMode;
            channel.AppendCommandData((byte*) &samplingMode, 4);
          }
          checked { ++index; }
        }
      }
      if (this._samplerCount > 0U)
      {
        int index = 0;
        while (index < this._samplerData.Count)
        {
          ShaderEffect.SamplerData? nullable = this._samplerData[index];
          if (nullable.HasValue)
          {
            ShaderEffect.SamplerData samplerData = nullable.Value;
            DUCE.ResourceHandle resourceHandle = samplerData._brush != null ? ((DUCE.IResource) samplerData._brush).GetHandle(channel) : DUCE.ResourceHandle.Null;
            channel.AppendCommandData((byte*) &resourceHandle, sizeof (DUCE.ResourceHandle));
          }
          checked { ++index; }
        }
      }
      channel.EndCommand();
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    private unsafe void AppendRegisters<T>(DUCE.Channel channel, List<T?> list) where T : struct
    {
      if (list == null)
        return;
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index].HasValue)
        {
          short num = (short) index;
          channel.AppendCommandData((byte*) &num, 2);
        }
      }
    }

    internal override DUCE.ResourceHandle AddRefOnChannelCore(DUCE.Channel channel)
    {
      if (this._duceResource.CreateOrAddRefOnChannel((object) this, channel, DUCE.ResourceType.TYPE_SHADEREFFECT))
      {
        if (this._samplerCount > 0U)
        {
          int count = this._samplerData.Count;
          for (int index = 0; index < count; ++index)
          {
            ShaderEffect.SamplerData? nullable = this._samplerData[index];
            if (nullable.HasValue)
              ((DUCE.IResource) nullable.Value._brush)?.AddRefOnChannel(channel);
          }
        }
        ((DUCE.IResource) this.PixelShader)?.AddRefOnChannel(channel);
        this.AddRefOnChannelAnimations(channel);
        this.UpdateResource(channel, true);
      }
      return this._duceResource.GetHandle(channel);
    }

    internal override void ReleaseOnChannelCore(DUCE.Channel channel)
    {
      if (!this._duceResource.ReleaseOnChannel(channel))
        return;
      if (this._samplerCount > 0U)
      {
        int count = this._samplerData.Count;
        for (int index = 0; index < count; ++index)
        {
          ShaderEffect.SamplerData? nullable = this._samplerData[index];
          if (nullable.HasValue)
            ((DUCE.IResource) nullable.Value._brush)?.ReleaseOnChannel(channel);
        }
      }
      ((DUCE.IResource) this.PixelShader)?.ReleaseOnChannel(channel);
      this.ReleaseOnChannelAnimations(channel);
    }

    internal static System.Type DetermineShaderConstantType(System.Type type, PixelShader pixelShader)
    {
      System.Type type1 = (System.Type) null;
      if (type == typeof (double) || type == typeof (float) || (type == typeof (Color) || type == typeof (Point)) || (type == typeof (Size) || type == typeof (Vector) || (type == typeof (Point3D) || type == typeof (Vector3D))) || type == typeof (Point4D))
        type1 = typeof (float);
      else if (pixelShader != null && pixelShader.ShaderMajorVersion >= (short) 3)
      {
        if (type == typeof (int) || type == typeof (uint) || (type == typeof (byte) || type == typeof (sbyte)) || (type == typeof (long) || type == typeof (ulong) || (type == typeof (short) || type == typeof (ushort))) || type == typeof (char))
          type1 = typeof (int);
        else if (type == typeof (bool))
          type1 = typeof (bool);
      }
      return type1;
    }

    internal static void ConvertValueToMilColorF(object value, out MilColorF newVal)
    {
      System.Type type = value.GetType();
      if (type == typeof (double) || type == typeof (float))
      {
        float num = type == typeof (double) ? (float) (double) value : (float) value;
        newVal.r = newVal.g = newVal.b = newVal.a = num;
      }
      else if (type == typeof (Color))
      {
        Color color = (Color) value;
        newVal.r = (float) color.R / (float) byte.MaxValue;
        newVal.g = (float) color.G / (float) byte.MaxValue;
        newVal.b = (float) color.B / (float) byte.MaxValue;
        newVal.a = (float) color.A / (float) byte.MaxValue;
      }
      else if (type == typeof (Point))
      {
        Point point = (Point) value;
        newVal.r = (float) point.X;
        newVal.g = (float) point.Y;
        newVal.b = 1f;
        newVal.a = 1f;
      }
      else if (type == typeof (Size))
      {
        Size size = (Size) value;
        newVal.r = (float) size.Width;
        newVal.g = (float) size.Height;
        newVal.b = 1f;
        newVal.a = 1f;
      }
      else if (type == typeof (Vector))
      {
        Vector vector = (Vector) value;
        newVal.r = (float) vector.X;
        newVal.g = (float) vector.Y;
        newVal.b = 1f;
        newVal.a = 1f;
      }
      else if (type == typeof (Point3D))
      {
        Point3D point3D = (Point3D) value;
        newVal.r = (float) point3D.X;
        newVal.g = (float) point3D.Y;
        newVal.b = (float) point3D.Z;
        newVal.a = 1f;
      }
      else if (type == typeof (Vector3D))
      {
        Vector3D vector3D = (Vector3D) value;
        newVal.r = (float) vector3D.X;
        newVal.g = (float) vector3D.Y;
        newVal.b = (float) vector3D.Z;
        newVal.a = 1f;
      }
      else if (type == typeof (Point4D))
      {
        Point4D point4D = (Point4D) value;
        newVal.r = (float) point4D.X;
        newVal.g = (float) point4D.Y;
        newVal.b = (float) point4D.Z;
        newVal.a = (float) point4D.W;
      }
      else
        newVal.r = newVal.b = newVal.g = newVal.a = 1f;
    }

    internal static void ConvertValueToMilColorI(object value, out MilColorI newVal)
    {
      System.Type type = value.GetType();
      int num = !(type == typeof (long)) ? (!(type == typeof (ulong)) ? (!(type == typeof (uint)) ? (!(type == typeof (short)) ? (!(type == typeof (ushort)) ? (!(type == typeof (byte)) ? (!(type == typeof (sbyte)) ? (!(type == typeof (char)) ? (int) value : (int) (char) value) : (int) (sbyte) value) : (int) (byte) value) : (int) (ushort) value) : (int) (short) value) : (int) (uint) value) : (int) (ulong) value) : (int) (long) value;
      newVal.r = newVal.g = newVal.b = newVal.a = num;
    }

    /// <summary>基本 (アニメーション化されていない) プロパティ値を使用して、インスタンスを、指定した <see cref="T:System.Windows.Freezable" /> の複製 (詳細コピー) にします。</summary>
    /// <param name="sourceFreezable">複製する対象のオブジェクト。</param>
    protected override void CloneCore(Freezable sourceFreezable)
    {
      ShaderEffect effect = (ShaderEffect) sourceFreezable;
      base.CloneCore(sourceFreezable);
      this.CopyCommon(effect);
    }

    /// <summary>現在のプロパティ値を使用して、インスタンスを、指定した <see cref="T:System.Windows.Freezable" /> の変更可能な複製 (詳細コピー) にします。</summary>
    /// <param name="sourceFreezable">複製する <see cref="T:System.Windows.Freezable" />。</param>
    protected override void CloneCurrentValueCore(Freezable sourceFreezable)
    {
      ShaderEffect effect = (ShaderEffect) sourceFreezable;
      base.CloneCurrentValueCore(sourceFreezable);
      this.CopyCommon(effect);
    }

    /// <summary>基本プロパティ値 (アニメーション化されていない値) を使用して、インスタンスを、指定した <see cref="T:System.Windows.Freezable" /> の固定された複製にします。</summary>
    /// <param name="sourceFreezable">コピーするインスタンス。</param>
    protected override void GetAsFrozenCore(Freezable sourceFreezable)
    {
      ShaderEffect effect = (ShaderEffect) sourceFreezable;
      base.GetAsFrozenCore(sourceFreezable);
      this.CopyCommon(effect);
    }

    /// <summary>現在のインスタンスを、指定した <see cref="T:System.Windows.Freezable" /> の固定された複製にします。 オブジェクトに、アニメーション化された依存関係プロパティが存在する場合、現在アニメーション化されている値がコピーされます。</summary>
    /// <param name="sourceFreezable">コピーし、固定する <see cref="T:System.Windows.Freezable" />。</param>
    protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
    {
      ShaderEffect effect = (ShaderEffect) sourceFreezable;
      base.GetCurrentValueAsFrozenCore(sourceFreezable);
      this.CopyCommon(effect);
    }

    private void CopyCommon(ShaderEffect effect)
    {
      this._topPadding = effect._topPadding;
      this._bottomPadding = effect._bottomPadding;
      this._leftPadding = effect._leftPadding;
      this._rightPadding = effect._rightPadding;
      if (this._floatRegisters != null)
        this._floatRegisters = new List<MilColorF?>((IEnumerable<MilColorF?>) effect._floatRegisters);
      if (this._samplerData != null)
        this._samplerData = new List<ShaderEffect.SamplerData?>((IEnumerable<ShaderEffect.SamplerData?>) effect._samplerData);
      this._floatCount = effect._floatCount;
      this._samplerCount = effect._samplerCount;
      this._ddxUvDdyUvRegisterIndex = effect._ddxUvDdyUvRegisterIndex;
    }

    /// <summary>この <see cref="T:System.Windows.Media.Effects.ShaderEffect" /> オブジェクトの変更可能な複製を作成し、このオブジェクトの値の詳細コピーを作成します。 このメソッドは、このオブジェクトの依存関係プロパティをコピーするときにリソース参照とデータ バインディングをコピーしますが (ただし、これらは解決されなくなる場合があります)、アニメーションやその現在の値はコピーしません。</summary>
    /// <returns>このインスタンスの変更可能な複製。 返される複製は、事実上、現在のオブジェクトの詳細コピーです。 複製の <see cref="P:System.Windows.Freezable.IsFrozen" /> プロパティは <see langword="false" /> です。</returns>
    public ShaderEffect Clone() => (ShaderEffect) base.Clone();

    /// <summary>この <see cref="T:System.Windows.Media.Effects.ShaderEffect" /> オブジェクトの変更可能な複製を作成し、このオブジェクトの現在値の詳細コピーを作成します。 リソース参照、データ バインディング、アニメーションはコピーされませんが、それらの現在値はコピーされます。</summary>
    /// <returns>現在のオブジェクトの変更可能な複製。 複製されたオブジェクトの <see cref="P:System.Windows.Freezable.IsFrozen" /> プロパティは、ソースの <see langword="false" /> プロパティが <see cref="P:System.Windows.Freezable.IsFrozen" /> であった場合でも、<see langword="true" /> になります。</returns>
    public ShaderEffect CloneCurrentValue() => (ShaderEffect) base.CloneCurrentValue();

    private static void PixelShaderPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ShaderEffect shaderEffect = (ShaderEffect) d;
      shaderEffect.PixelShaderPropertyChangedHook(e);
      if (e.IsASubPropertyChange && e.OldValueSource == e.NewValueSource)
        return;
      PixelShader oldValue = (PixelShader) e.OldValue;
      PixelShader newValue = (PixelShader) e.NewValue;
      if (shaderEffect.Dispatcher != null)
      {
        DUCE.IResource resource = (DUCE.IResource) shaderEffect;
        using (CompositionEngineLock.Acquire())
        {
          int channelCount = resource.GetChannelCount();
          for (int index = 0; index < channelCount; ++index)
          {
            DUCE.Channel channel = resource.GetChannel(index);
            shaderEffect.ReleaseResource((DUCE.IResource) oldValue, channel);
            shaderEffect.AddRefResource((DUCE.IResource) newValue, channel);
          }
        }
      }
      shaderEffect.PropertyChanged(ShaderEffect.PixelShaderProperty);
    }

    /// <summary>効果で使用する <see cref="T:System.Windows.Media.Effects.PixelShader" /> を取得または設定します。</summary>
    /// <returns>効果の <see cref="T:System.Windows.Media.Effects.PixelShader" />。</returns>
    protected PixelShader PixelShader
    {
      get => (PixelShader) this.GetValue(ShaderEffect.PixelShaderProperty);
      set => this.SetValueInternal(ShaderEffect.PixelShaderProperty, (object) value);
    }

    /// <summary>
    /// <see cref="T:System.Windows.Freezable" /> 派生クラスの新しいインスタンスを作成します。</summary>
    /// <returns>新しいインスタンス。</returns>
    protected override Freezable CreateInstanceCore() => (Freezable) Activator.CreateInstance(this.GetType());

    [SecurityCritical]
    [SecurityTreatAsSafe]
    internal override void UpdateResource(DUCE.Channel channel, bool skipOnChannelCheck)
    {
      this.ManualUpdateResource(channel, skipOnChannelCheck);
      base.UpdateResource(channel, skipOnChannelCheck);
    }

    private DUCE.ResourceHandle GeneratedAddRefOnChannelCore(DUCE.Channel channel)
    {
      if (this._duceResource.CreateOrAddRefOnChannel((object) this, channel, DUCE.ResourceType.TYPE_SHADEREFFECT))
      {
        ((DUCE.IResource) this.PixelShader)?.AddRefOnChannel(channel);
        this.AddRefOnChannelAnimations(channel);
        this.UpdateResource(channel, true);
      }
      return this._duceResource.GetHandle(channel);
    }

    private void GeneratedReleaseOnChannelCore(DUCE.Channel channel)
    {
      if (!this._duceResource.ReleaseOnChannel(channel))
        return;
      ((DUCE.IResource) this.PixelShader)?.ReleaseOnChannel(channel);
      this.ReleaseOnChannelAnimations(channel);
    }

    internal override DUCE.ResourceHandle GetHandleCore(DUCE.Channel channel) => this._duceResource.GetHandle(channel);

    internal override int GetChannelCountCore() => this._duceResource.GetChannelCount();

    internal override DUCE.Channel GetChannelCore(int index) => this._duceResource.GetChannel(index);

    private struct SamplerData
    {
      public Brush _brush;
      public SamplingMode _samplingMode;
    }
  }
}
