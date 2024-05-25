using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace K8055Velleman;

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

	public delegate void onConnectionChanged();
	public static event onConnectionChanged OnConnectionChanged;

	public delegate void onDigitalChannelsChange(DigitalChannel digitalChannel);
	public static event onDigitalChannelsChange OnDigitalChannelsChange;

    public delegate void onAnalogChannelsChange(AnalogChannel analogChannel, int value);
    public static event onAnalogChannelsChange OnAnalogChannelsChange;

    public static readonly List<int> ConnectedDevices = [];

	public static bool IsConnected { get { return ConnectedDevices.Contains(CurrentDevice); } }
	
	public static int CurrentDevice { get; private set; } = -1;//IsConnected ? CurrentDevice : -1;

	private static readonly List<DigitalChannel> s_digitalChannels = [];
	private static readonly Dictionary<AnalogChannel, int> s_analogChannels = [];

	public static void Update()
	{
		bool oldIsConnected = IsConnected;
		UpdateConnectedDevice();
		if (!IsConnected)
		{
			SearchAndOpenDevice();
			//if(!IsConnected) return;
		}

		if (IsConnected != oldIsConnected)
		{
			OnConnectionChanged?.Invoke();
		}

		if(!IsConnected) return;

		UpdateDigitalsChannel();
		UpdateAnalogChannel();

		//if((digitalChannel & 1) > 0)
	}

	public static int OpenDevice(int CardAddress)
	{
		if(ConnectedDevices.Contains(CardAddress)) return SetCurrentDevice(CardAddress);
		CurrentDevice = Interface.OpenDevice(CardAddress);

		if (CurrentDevice >= 0)
		{
			ConnectedDevices.Add(CurrentDevice);
			Reset();
		}
		return CurrentDevice;
	}

	public static bool CloseDevice(int CardAddress) 
	{
		int tempCurrentDevice = CurrentDevice;
		if (CardAddress >= 0 && ConnectedDevices.Contains(CardAddress))
		{
			if(Interface.SetCurrentDevice(CardAddress) >= 0)
			{
				Reset();
				CloseDevice();
				Interface.SetCurrentDevice(tempCurrentDevice);
				return true;
			}
			Interface.SetCurrentDevice(tempCurrentDevice);
		}
		return false;
	}

	public static void CloseDevice()
	{
		if(IsConnected)
		{
			Reset();
			Interface.CloseDevice();
			ConnectedDevices.Remove(CurrentDevice);
			CurrentDevice = -1;
		}
	}

	public static void CloseAllDevices()
	{
		List<int> temp = new(ConnectedDevices);
		foreach(int CardAddress in temp)
		{
			if(Interface.SetCurrentDevice(CardAddress) >= 0)
			{
				Reset();
				CloseDevice();
			}
		}
	}

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
			if (Interface.SetCurrentDevice(i) >= 0) continue; 
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

	public static void ClearDigitalChannel(DigitalChannel channel)
	{
		if (!IsConnected || channel >= 0) return;
		s_digitalChannels.Remove(channel);
		Interface.ClearDigitalChannel(-(int)channel);
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

	private static void UpdateConnectedDevice()
	{
		if (ConnectedDevices.Count <= 0) return;
		ConnectedDevices.Clear();
		for (int i = 0; i < 4; i++)
		{
			if (Interface.SetCurrentDevice(i) >= 0) ConnectedDevices.Add(i);
		}
		CurrentDevice = Interface.SetCurrentDevice(CurrentDevice);
	}

	private static void UpdateDigitalsChannel()
	{
		int digitalChannel = Interface.ReadAllDigital();
		//digitalChannels[CurrentDevice].RemoveAll(new Predicate<DigitalChannel>((e) => { return e == DigitalChannel.B1 || e == DigitalChannel.B2 || e == DigitalChannel.B3 || e == DigitalChannel.B4 || e == DigitalChannel.B5; } ));
		if ((digitalChannel & 1) > 0)
		{
			if (!s_digitalChannels.Contains(DigitalChannel.I1))
			{
				s_digitalChannels.Add(DigitalChannel.I1);
				OnDigitalChannelsChange?.Invoke(DigitalChannel.I1);
			}

		}
		else 
		{
			if (s_digitalChannels.Contains(DigitalChannel.I1))
			{
				s_digitalChannels.Remove(DigitalChannel.I1);
			}
		}

		if ((digitalChannel & 2) > 0)
		{
			if (!s_digitalChannels.Contains(DigitalChannel.I2))
			{
				s_digitalChannels.Add(DigitalChannel.I2);
				OnDigitalChannelsChange?.Invoke(DigitalChannel.I2);
			}

		}
		else 
		{
			if (s_digitalChannels.Contains(DigitalChannel.I2))
			{
				s_digitalChannels.Remove(DigitalChannel.I2);
			}
		}

		if ((digitalChannel & 4) > 0)
		{
			if (!s_digitalChannels.Contains(DigitalChannel.I3))
			{
				s_digitalChannels.Add(DigitalChannel.I3);
				OnDigitalChannelsChange?.Invoke(DigitalChannel.I3);
			}

		}
		else
		{
			if (s_digitalChannels.Contains(DigitalChannel.I3))
			{
				s_digitalChannels.Remove(DigitalChannel.I3);
			}
		}

		if ((digitalChannel & 8) > 0)
		{
			if (!s_digitalChannels.Contains(DigitalChannel.I4))
			{
				s_digitalChannels.Add(DigitalChannel.I4);
				OnDigitalChannelsChange?.Invoke(DigitalChannel.I4);
			}

		}
		else
		{
			if (s_digitalChannels.Contains(DigitalChannel.I4))
			{
				s_digitalChannels.Remove(DigitalChannel.I4);
			}
		}

		if ((digitalChannel & 16) > 0)
		{
			if (!s_digitalChannels.Contains(DigitalChannel.I5))
			{
				s_digitalChannels.Add(DigitalChannel.I5);
				OnDigitalChannelsChange?.Invoke(DigitalChannel.I5);
			}

		}
		else
		{
			if (s_digitalChannels.Contains(DigitalChannel.I5))
			{
				s_digitalChannels.Remove(DigitalChannel.I5);
			}
		}
	}

	private static void UpdateAnalogChannel()
	{
		int data1 = 0;
		int data2 = 0;
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

