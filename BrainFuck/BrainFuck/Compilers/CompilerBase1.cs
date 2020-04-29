using BrainFuck.VMs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.Emit.OpCodes;

namespace BrainFuck.Compilers
{
    public abstract class CompilerBase1
    {
        private static readonly AssemblyBuilder AsmBuilder;

        private static readonly ModuleBuilder ModuleBuilder;

        private static readonly MethodInfo DataPointerGet =
            typeof(BFVM1).GetProperty("DataPointer", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(nonPublic:true);
        private static readonly MethodInfo DataPointerSet =
            typeof(BFVM1).GetProperty("DataPointer", BindingFlags.NonPublic | BindingFlags.Instance).GetSetMethod(nonPublic: true);

        private static readonly MethodInfo DataGet =
            typeof(BFVM1).GetProperty("Data", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(nonPublic: true);
        private static readonly MethodInfo DataSet =
            typeof(BFVM1).GetProperty("Data", BindingFlags.NonPublic | BindingFlags.Instance).GetSetMethod(nonPublic: true);

        private static readonly MethodInfo ReadByte =
            typeof(Stream).GetMethod(nameof(Stream.ReadByte));

        private static readonly MethodInfo WriteByte =
            typeof(Stream).GetMethod(nameof(Stream.WriteByte));

        private static string MakeRandomNamePart() =>
            Guid.NewGuid().ToString("N");
        static CompilerBase1()
        {
            var asmName =
                new AssemblyName("TempAsm" + MakeRandomNamePart());
            AsmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AsmBuilder.DefineDynamicModule("TempModule" + MakeRandomNamePart());
        }

        private readonly Stack<Label> LBracketStack = new Stack<Label>();
        private readonly Stack<Label> RBracketStack = new Stack<Label>();
        private readonly ILGenerator IL;

        private readonly LocalBuilder IntTmp;
        private readonly LocalBuilder LongTmp;
        private readonly LocalBuilder UintTmp;

        public RunnerFactory Result { get; }

        public CompilerBase1(string source):
            this(source, "Program" + MakeRandomNamePart())
        {

        }

        public CompilerBase1(string source, string name)
        {
            var typeBuilder = ModuleBuilder.DefineType(name, TypeAttributes.Class | TypeAttributes.Public, typeof(BFVM1));
            var methodBuilder = typeBuilder.DefineMethod(
                nameof(IRunner.Execute),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                typeof(void),
                new[] { typeof(Stream), typeof(Stream) }
                );

            methodBuilder.DefineParameter(1, ParameterAttributes.None, "input");
            methodBuilder.DefineParameter(2, ParameterAttributes.None, "output");

            IL = methodBuilder.GetILGenerator();

            IntTmp = IL.DeclareLocal(typeof(int));
            LongTmp = IL.DeclareLocal(typeof(long));
            UintTmp = IL.DeclareLocal(typeof(byte));

            var code = Preprocess(source);
            foreach(var instruction in code)
            {
                switch(instruction)
                {
                    case Instructions.LShift:
                        InjectLShift();
                        break;
                    case Instructions.RShift:
                        InjectRShift();
                        break;
                    case Instructions.Inc:
                        InjectInc();
                        break;
                    case Instructions.Dec:
                        InjectDec();
                        break;
                    case Instructions.Read:
                        InjectRead();
                        break;
                    case Instructions.Write:
                        InjectWrite();
                        break;
                    case Instructions.LBracket:
                        InjectLBracket();
                        break;
                    case Instructions.RBracket:
                        InjectRBracket();
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            IL.Emit(Ret);
            var type = typeBuilder.CreateType();
            Result = new RunnerFactory(type);
        }

        protected abstract IEnumerable<Instructions> Preprocess(string source);

        private void InjectLShift()
        {
            IL.Emit(Ldarg_0);
            IL.Emit(Call, DataPointerGet);
            IL.Emit(Stloc, LongTmp);
            IL.Emit(Ldarg_0);
            IL.Emit(Ldloc, LongTmp);
            IL.Emit(Ldc_I4_1);
            IL.Emit(Conv_I8);
            IL.Emit(Sub);
            IL.Emit(Call, DataPointerSet);
        }

        private void InjectRShift()
        {
            IL.Emit(Ldarg_0);
            IL.Emit(Call, DataPointerGet);
            IL.Emit(Stloc, LongTmp);
            IL.Emit(Ldarg_0);
            IL.Emit(Ldloc, LongTmp);
            IL.Emit(Ldc_I4_1);
            IL.Emit(Conv_I8);
            IL.Emit(Add);
            IL.Emit(Call, DataPointerSet);
        }

        private void InjectInc()
        {
            IL.Emit(Ldarg_0);
            IL.Emit(Call, DataGet);
            IL.Emit(Stloc, UintTmp);
            IL.Emit(Ldarg_0);
            IL.Emit(Ldloc, UintTmp);
            IL.Emit(Ldc_I4_1);
            IL.Emit(Add);
            IL.Emit(Conv_U1);
            IL.Emit(Call, DataSet);
        }

        private void InjectDec()
        {
            IL.Emit(Ldarg_0);
            IL.Emit(Call, DataGet);
            IL.Emit(Stloc, UintTmp);
            IL.Emit(Ldarg_0);
            IL.Emit(Ldloc, UintTmp);
            IL.Emit(Ldc_I4_1);
            IL.Emit(Sub);
            IL.Emit(Conv_U1);
            IL.Emit(Call, DataSet);
        }

        private void InjectRead()
        {
            var lbEq = IL.DefineLabel();
            var lbEndif = IL.DefineLabel();
            IL.Emit(Ldarg_1);
            IL.Emit(Callvirt, ReadByte);
            IL.Emit(Stloc, IntTmp);

            IL.Emit(Ldarg_0);
            IL.Emit(Ldc_I4_M1);
            IL.Emit(Ldloc, IntTmp);
            IL.Emit(Beq, lbEq);
            IL.Emit(Ldloc, IntTmp);
            IL.Emit(Conv_U1);
            IL.Emit(Br, lbEndif);
            IL.MarkLabel(lbEq);
            IL.Emit(Ldc_I4_0);
            IL.MarkLabel(lbEndif);
            IL.Emit(Call, DataSet);
        }

        private void InjectWrite()
        {
            IL.Emit(Ldarg_2);
            IL.Emit(Ldarg_0);
            IL.Emit(Call, DataGet);
            IL.Emit(Callvirt, WriteByte);
        }

        private void InjectLBracket()
        {
            var lbBeforeBegin = IL.DefineLabel();
            LBracketStack.Push(lbBeforeBegin);
            var lbAfterEnd = IL.DefineLabel();
            RBracketStack.Push(lbAfterEnd);
            IL.MarkLabel(lbBeforeBegin);
            IL.Emit(Ldc_I4_0);
            IL.Emit(Conv_U1);
            IL.Emit(Ldarg_0);
            IL.Emit(Call, DataGet);
            IL.Emit(Beq, lbAfterEnd);
        }

        private void InjectRBracket()
        {
            var lbBeforeBegin = LBracketStack.Pop();
            var lbAfterEnd = RBracketStack.Pop();
            IL.Emit(Br, lbBeforeBegin);
            IL.MarkLabel(lbAfterEnd);
        }
    }
}
