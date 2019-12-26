using System;
using BrainFuck;

class MainClass {
  public static void Main (string[] args) {
    var arr = new InfiniArray<byte>();
    Console.WriteLine (arr[348]);
    arr[9897]=22;
    Console.WriteLine(arr[9897]);
  }
}