using System;
namespace kitchen
{
    public class Utility
    {
        public static int wrapIndex(int i, int i_max)
        {
            return ((i % i_max) + i_max) % i_max;
        }
		public static void Swap<T>(ref T A, ref T B)
		{
			T tmp = B;
			A = B;
			B = tmp;
		}
		//public static void Swap<T>(ref T A, ref T B)
		//{
		//    (A, B) = (B, A);
		//}


		public static string getFullPath(string relativePath) {
			string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
			return System.IO.Path.Combine(baseDirectory, relativePath);
		}
	}
}
