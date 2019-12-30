using System;
using BrainFuck;
using System.Diagnostics;

class MainClass {
  
  public static void Main (string[] args) 
  {
    //var code = "+++[-]";
    var hello =@"++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";
    var program = new BFInterpreter(hello);
    program.Execute(Console.OpenStandardInput(), Console.OpenStandardOutput());
    //var arr = new InfiniArray<byte>();
    //for(int i = 1; i< 999999;i*=2)
    //{
      //Console.WriteLine($"{i}:{arr[i]=(byte)i}");
    //}
  }
}