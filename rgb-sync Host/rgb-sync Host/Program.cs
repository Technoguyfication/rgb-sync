using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using Corale.Colore;
using Corale.Colore.Core;
using System.Diagnostics;
using System.IO;

namespace rgb_sync_Host
{
	class Program
	{
		private const double SYNAPSE_UPDATE_INTERVAL = 1000d / 30;
		private const double ARDUINO_UPDATE_INTERVAL = 1000d / 15;

		private static Timer _synapseTimer = new Timer(SYNAPSE_UPDATE_INTERVAL);
		private static Timer _arduinoTimer = new Timer(ARDUINO_UPDATE_INTERVAL);
		private static SerialPort _arduinoPort;
		private static Stopwatch _syncStopwatch = new Stopwatch();
		private static CancellationTokenSource _cts = new CancellationTokenSource();

		private static float _spectrumSpeed = 0.05f;

		static void Main(string[] args)
		{
			// register timer events
			_synapseTimer.Elapsed += _synapseTimer_Elapsed;
			_arduinoTimer.Elapsed += _arduinoTimer_Elapsed;

			_syncStopwatch.Start();
			Console.WriteLine($"Color speed: {_spectrumSpeed}");

			if (Chroma.SdkAvailable)
			{
				Console.WriteLine($"Chroma SDK Version: {Chroma.Instance.SdkVersion}");
				_synapseTimer.Start();
				Console.WriteLine($"Chroma refresh rate: {SYNAPSE_UPDATE_INTERVAL.ToString("0.00")}");
			}
			else
			{
				Console.WriteLine("Chroma SDK not available!");
				return;
			}

			_arduinoPort = new SerialPort("COM3");
			_arduinoPort.BaudRate = 115200;
			try
			{
				_arduinoPort.Open();
				_arduinoTimer.Start();
			}
			catch (IOException)
			{
				Console.WriteLine();
			}

			_cts.Token.WaitHandle.WaitOne();
		}

		private static void _arduinoTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Color col = HSVToRGB(GetH(), 1f, 1f);
			byte[] data = new byte[] { col.R, col.G, col.B };
			_arduinoPort.Write(data, 0, 3);
		}

		/// <summary>
		/// Gets the current H value for this moment
		/// </summary>
		/// <returns></returns>
		private static float GetH()
		{
			return _syncStopwatch.ElapsedMilliseconds * _spectrumSpeed % 1000 / 1000f;	// get a number between 0f and 1f using the elapsed ms and spectrum speed
		}

		private static void _synapseTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Chroma.Instance.SetAll(HSVToRGB(GetH(), 1f, 1f));
		}

		/// <summary>
		/// Converts HSV to RGB
		/// </summary>
		/// <param name="H"></param>
		/// <param name="S"></param>
		/// <param name="V"></param>
		/// <returns></returns>
		public static Color HSVToRGB(float H, float S, float V)
		{
			// https://en.wikipedia.org/wiki/HSL_and_HSV#From_HSV

			if (S == 0f)    // no saturation - only use brightness value
				return new Color(V, V, V);
			else if (V == 0f)   // no brightness - black
				return Color.Black;
			else
			{
				Color col = Color.Black;
				float Hval = H * 6f;
				int sel = (int)Math.Floor(Hval);
				float mod = Hval - sel;
				float v1 = V * (1f - S);
				float v2 = V * (1f - S * mod);
				float v3 = V * (1f - S * (1f - mod));
				switch (sel + 1)
				{
					case 0:
						col = new Color(V, v1, v2);
						break;
					case 1:
						col = new Color(V, v3, v1);
						break;
					case 2:
						col = new Color(v2, V, v1);
						break;
					case 3:
						col = new Color(v1, V, v3);
						break;
					case 4:
						col = new Color(v1, v2, V);
						break;
					case 5:
						col = new Color(v3, v1, V);
						break;
					case 6:
						col = new Color(V, v1, v2);
						break;
					case 7:
						col = new Color(V, v3, v1);
						break;
				}
				return col;
			}
		}
	}
}
