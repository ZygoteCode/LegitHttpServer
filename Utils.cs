namespace LegitHttpServer
{
	using System.Collections.Generic;
	using System.IO;
	using System.Net.Sockets;
	using System;
	using System.Diagnostics;
	using System.Text;

	public class Utils
	{
		public static IEnumerable<string> SplitToLines(string input)
		{
			if (input == null)
			{
				yield break;
			}

			using (System.IO.StringReader reader = new System.IO.StringReader(input))
			{
				string line;

				while ((line = reader.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}

		public static string NetworkStreamToString(NetworkStream stream)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] data = new byte[256];
				int size;

				do
				{
					size = stream.Read(data, 0, data.Length);

					if (size == 0)
					{
						return null;
					}

					memoryStream.Write(data, 0, size);
				}
				while (stream.DataAvailable);

				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
		}

		public static byte[] NetworkStreamToBytes(NetworkStream stream)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] data = new byte[256];
				int size;

				do
				{
					size = stream.Read(data, 0, data.Length);

					if (size == 0)
					{
						return null;
					}

					memoryStream.Write(data, 0, size);
				}
				while (stream.DataAvailable);

				return memoryStream.ToArray();
			}
		}

		public static byte[] Combine(byte[] first, byte[] second)
		{
			byte[] ret = new byte[first.Length + second.Length];
			Buffer.BlockCopy(first, 0, ret, 0, first.Length);
			Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
			return ret;
		}

		public static byte[] Combine(byte[] first, byte[] second, byte[] third)
		{
			byte[] ret = new byte[first.Length + second.Length + third.Length];
			Buffer.BlockCopy(first, 0, ret, 0, first.Length);
			Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
			Buffer.BlockCopy(third, 0, ret, first.Length + second.Length,
							 third.Length);
			return ret;
		}
	}
}