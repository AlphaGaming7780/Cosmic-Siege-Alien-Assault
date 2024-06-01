using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace K8055Velleman;

/// <summary>
/// Static class that can be used to comunicate with the Velleman board.
/// </summary>
static public class K8055
{
	private class Interface
	{
		private const string PathToK8055 = "Resources\\K8055D.dll";
		#region k08055
		[DllImport(PathToK8055)]
		public static extern int OpenDevice(int CardAddress);
		[DllImport(PathToK8055)]
		public static extern void CloseDevice();
		[DllImport(PathToK8055)]
		public static extern int ReadAnalogChannel(int Channel);
		[DllImport(PathToK8055)]
		public static extern void ReadAllAnalog(ref int Data1, ref int Data2);
		[DllImport(PathToK8055)]
		public static extern void OutputAnalogChannel(int Channel, int Data);
		[DllImport(PathToK8055)]
		public static extern void OutputAllAnalog(int Data1, int Data2);
		[DllImport(PathToK8055)]
		public static extern void ClearAnalogChannel(int Channel);
		[DllImport(PathToK8055)]
		public static extern void SetAllAnalog();
		[DllImport(PathToK8055)]
		public static extern void ClearAllAnalog();
		[DllImport(PathToK8055)]
		public static extern void SetAnalogChannel(int Channel);
		[DllImport(PathToK8055)]
		public static extern void WriteAllDigital(int Data);
		[DllImport(PathToK8055)]
		public static extern void ClearDigitalChannel(int Channel);
		[DllImport(PathToK8055)]
		public static extern void ClearAllDigital();
		[DllImport(PathToK8055)]
		public static extern void SetDigitalChannel(int Channel);

		[DllImport(PathToK8055)]
		public static extern void SetAllDigital();
		[DllImport(PathToK8055)]
		public static extern bool ReadDigitalChannel(int Channel);
		[DllImport(PathToK8055)]
		public static extern int ReadAllDigital();
		[DllImport(PathToK8055)]
		public static extern int ReadCounter(int CounterNr);
		[DllImport(PathToK8055)]
		public static extern void ResetCounter(int CounterNr);
		[DllImport(PathToK8055)]
		public static extern void SetCounterDebounceTime(int CounterNr, int DebounceTime);
		[DllImport(PathToK8055)]
		public static extern int Version();
		[DllImport(PathToK8055)]
		public static extern int SearchDevices();
		[DllImport(PathToK8055)]
		public static extern int SetCurrentDevice(int lngCardAddress);
		#endregion
	}

	public enum DigitalChannel : int
	{
		O1 = -1,
		O2 = -2,
		O3 = -3,
		O4 = -4,
		O5 = -5,
		O6 = -6,
		O7 = -7,
		O8 = -8,
		I1 = 1,
		I2 = 2,
		I3 = 3,
		I4 = 4,
		I5 = 5,
	}

	public enum AnalogChannel : int
	{
		O2 = -2,
		O1 = -1,
		I1 = 1,
		I2 = 2,
	}

    /// <summary>
    /// Type for OnConnectionChanged event.
    /// </summary>
    public delegate void onConnectionChanged();
	/// <summary>
	/// An event triggerd when the connection changed with the current Velleman board.
	/// </summary>
	public static event onConnectionChanged OnConnectionChanged;

    /// <summary>
    /// The type for the OnDigitalChannelsChange event.
    /// </summary>
    /// <param name="digitalChannel">The digital channel that changed.</param>
    public delegate void onDigitalChannelsChange(DigitalChannel digitalChannel);
    /// <summary>
    /// An event triggered when a digital channel changed value.
    /// </summary>
    public static event onDigitalChannelsChange OnDigitalChannelsChange;

	/// <summary>
	/// The type for the OnAnalogChannelsChange event.
	/// </summary>
	/// <param name="analogChannel">The analog channel that changed.</param>
	/// <param name="value">The new value of the channel.</param>
	public delegate void onAnalogChannelsChange(AnalogChannel analogChannel, int value);
    /// <summary>
    /// An event triggered when an analog channel changed value.
    /// </summary>
    public static event onAnalogChannelsChange OnAnalogChannelsChange;

	//public static readonly List<int> ConnectedDevices = [];
	//public static bool IsConnected { get { return ConnectedDevices.Contains(CurrentDevice); } }
	public static bool IsConnected { get { return CurrentDevice != -1; } }

    public static int CurrentDevice { get; private set; } = -1;

	private static readonly List<DigitalChannel> s_digitalChannels = [];
	private static readonly Dictionary<AnalogChannel, int> s_analogChannels = [];

	public static void Update()
	{
		bool oldIsConnected = IsConnected;
		//UpdateConnectedDevice();
		CurrentDevice = Interface.SetCurrentDevice(CurrentDevice);
		if (!IsConnected)
		{
			SearchAndOpenDevice();
		}

		if (IsConnected != oldIsConnected)
		{
			OnConnectionChanged?.Invoke();
		}

		if(!IsConnected) return;

		UpdateDigitalsChannel();
		UpdateAnalogChannel();
	}

	public static int OpenDevice(int CardAddress)
	{
		//if(ConnectedDevices.Contains(CardAddress)) return SetCurrentDevice(CardAddress);
		CurrentDevice = Interface.OpenDevice(CardAddress);

		if (CurrentDevice >= 0)
		{
			//ConnectedDevices.Add(CurrentDevice);
			Reset();
		}
		return CurrentDevice;
	}

	//public static bool CloseDevice(int CardAddress) 
	//{
	//	int tempCurrentDevice = CurrentDevice;
	//	if (CardAddress >= 0 && ConnectedDevices.Contains(CardAddress))
	//	{
	//		if(Interface.SetCurrentDevice(CardAddress) >= 0)
	//		{
	//			Reset();
	//			CloseDevice();
	//			Interface.SetCurrentDevice(tempCurrentDevice);
	//			return true;
	//		}
	//		Interface.SetCurrentDevice(tempCurrentDevice);
	//	}
	//	return false;
	//}

	public static void CloseDevice()
	{
		if(IsConnected)
		{
			Reset();
			Interface.CloseDevice();
			//ConnectedDevices.Remove(CurrentDevice);
			CurrentDevice = -1;
		}
	}

	//public static void CloseAllDevices()
	//{
	//	List<int> temp = new(ConnectedDevices);
	//	foreach(int CardAddress in temp)
	//	{
	//		if(Interface.SetCurrentDevice(CardAddress) >= 0)
	//		{
	//			Reset();
	//			CloseDevice();
	//		}
	//	}
	//}

	public static int SetCurrentDevice(int CardAddress)
	{
		if(Interface.SetCurrentDevice(CardAddress) >= 0)
		{
			CurrentDevice = CardAddress;
			UpdateAnalogChannel();
			UpdateDigitalsChannel();
			return CurrentDevice;
		}
		return -1;
	}

	public static int SearchAndOpenDevice()
	{
		int x = -1;
		for(int i = 0; i < 4; i++)
		{
			if (SetCurrentDevice(i) >= 0)
			{
				return i;
			};
			x = OpenDevice(i);
			if(x >= 0)
			{
				return x;
			}
		}

		return x;
		
	}

	public static void Reset()
	{
		s_digitalChannels.Clear();
		s_analogChannels.Clear();
		Interface.ClearAllDigital();
		Interface.ClearAllAnalog();
	}

	public static bool ReadDigitalChannel(DigitalChannel channel)
	{   
		if(!IsConnected) return false;
		return s_digitalChannels.Contains(channel);
	}

	public static void SetDigitalChannel(DigitalChannel channel)
	{
		if(!IsConnected || channel >= 0) return;
		if(!s_digitalChannels.Contains(channel)) s_digitalChannels.Add(channel);

		Interface.SetDigitalChannel(-(int)channel);
	}

	public static void WriteAllDigital(int data)
	{
		if ((data & 1) > 0 && !s_digitalChannels.Contains(DigitalChannel.O1)) s_digitalChannels.Add(DigitalChannel.O1);
		else s_digitalChannels.Remove(DigitalChannel.O1);

		if((data & 2) > 0 && !s_digitalChannels.Contains(DigitalChannel.O2)) s_digitalChannels.Add(DigitalChannel.O2);
        else s_digitalChannels.Remove(DigitalChannel.O2);

        if ((data & 4) > 0 && !s_digitalChannels.Contains(DigitalChannel.O3)) s_digitalChannels.Add(DigitalChannel.O3);
        else s_digitalChannels.Remove(DigitalChannel.O3);

        if ((data & 8) > 0 && !s_digitalChannels.Contains(DigitalChannel.O4)) s_digitalChannels.Add(DigitalChannel.O4);
        else s_digitalChannels.Remove(DigitalChannel.O4);

        if ((data & 16) > 0 && !s_digitalChannels.Contains(DigitalChannel.O5)) s_digitalChannels.Add(DigitalChannel.O5);
        else s_digitalChannels.Remove(DigitalChannel.O5);

        if ((data & 32) > 0 && !s_digitalChannels.Contains(DigitalChannel.O6)) s_digitalChannels.Add(DigitalChannel.O6);
        else s_digitalChannels.Remove(DigitalChannel.O6);

        if ((data & 64) > 0 && !s_digitalChannels.Contains(DigitalChannel.O7)) s_digitalChannels.Add(DigitalChannel.O7);
        else s_digitalChannels.Remove(DigitalChannel.O7);

        if ((data & 128) > 0 && !s_digitalChannels.Contains(DigitalChannel.O8)) s_digitalChannels.Add(DigitalChannel.O8);
        else s_digitalChannels.Remove(DigitalChannel.O8);

        Interface.WriteAllDigital(data);
    }

    public static void ClearDigitalChannel(DigitalChannel channel)
	{
		if (!IsConnected || channel >= 0) return;
		s_digitalChannels.Remove(channel);
		Interface.ClearDigitalChannel(-(int)channel);
	}

	public static void ClearAllDigital()
	{
		for(int i = -1; i >= -8; i--)
		{
			s_digitalChannels.Remove((DigitalChannel)i);
		}
		Interface.ClearAllDigital();
	}

    public static void SetAnalogChannel(AnalogChannel channel)
	{
		if (!IsConnected || channel >= 0) return;
		if (s_analogChannels.ContainsKey(channel)) s_analogChannels[channel] = 255;
		else s_analogChannels.Add(channel, 255);

		Interface.SetAnalogChannel(-(int)channel);
	}

	public static void ClearAnalogChannel(AnalogChannel channel)
	{
		if (!IsConnected || channel >= 0) return;
		s_analogChannels.Remove(channel);
		Interface.ClearAnalogChannel(-(int)channel);
	}

	public static int ReadAnalogChannel(AnalogChannel channel)
	{
		if (!IsConnected) return -1;
		//if (!s_analogChannels.ContainsKey(channel)) return -1;
		return s_analogChannels[channel];
	}

	public static void OutputAnalogChannel(AnalogChannel channel, int data)
	{
		if (!IsConnected || channel >= 0) return;
		if (s_analogChannels.ContainsKey(channel)) s_analogChannels[channel] = data;
		else s_analogChannels.Add(channel, data);
		Interface.OutputAnalogChannel(-(int)channel, data);
	}

	//private static void UpdateConnectedDevice()
	//{
	//	if (ConnectedDevices.Count <= 0) return;
	//	ConnectedDevices.Clear();
	//	for (int i = 0; i < 4; i++)
	//	{
	//		if (Interface.SetCurrentDevice(i) >= 0) ConnectedDevices.Add(i);
	//	}
	//	CurrentDevice = Interface.SetCurrentDevice(CurrentDevice);
	//}

	private static void UpdateDigitalsChannel()
	{
		int digitalChannel = Interface.ReadAllDigital();

        DigitalChannel dC = DigitalChannel.I1;
		for(int i = 1; i <= 16; i *= 2)
		{
            if ((digitalChannel & i) > 0)
            {
				
                if (!s_digitalChannels.Contains(dC))
                {
                    s_digitalChannels.Add(dC);
                    OnDigitalChannelsChange?.Invoke(dC);
                }

            }
            else if (s_digitalChannels.Contains(dC))
            {
                s_digitalChannels.Remove(dC);
            }
			dC++;
        }
	}

	private static void UpdateAnalogChannel()
	{
		if(Interface.SetCurrentDevice(CurrentDevice) < 0) return;
		int data1 = s_analogChannels.ContainsKey(AnalogChannel.I1) ? s_analogChannels[AnalogChannel.I1] : 0;
		int data2 = s_analogChannels.ContainsKey(AnalogChannel.I2) ? s_analogChannels[AnalogChannel.I2] : 0;
        Interface.ReadAllAnalog(ref data1,ref data2);
		if (s_analogChannels.ContainsKey(AnalogChannel.I1))
		{
			if (s_analogChannels[AnalogChannel.I1] != data1) OnAnalogChannelsChange?.Invoke(AnalogChannel.I1, data1);
			s_analogChannels[AnalogChannel.I1] = data1;
		}
		else
		{
            OnAnalogChannelsChange?.Invoke(AnalogChannel.I1, data1);
            s_analogChannels.Add(AnalogChannel.I1, data1);
		}

		if (s_analogChannels.ContainsKey(AnalogChannel.I2))
		{
			if (s_analogChannels[AnalogChannel.I2] != data1) OnAnalogChannelsChange?.Invoke(AnalogChannel.I2, data2);
			s_analogChannels[AnalogChannel.I2] = data2;
		}
		else
		{
            OnAnalogChannelsChange?.Invoke(AnalogChannel.I2, data1);
            s_analogChannels.Add(AnalogChannel.I2, data2);
		}
	}
}

