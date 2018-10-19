``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.286 (1803/April2018Update/Redstone4)
AMD Ryzen 7 1800X Eight-Core Processor (Max: 3.60GHz), 1 CPU, 16 logical and 8 physical cores
Frequency=3509027 Hz, Resolution=284.9793 ns, Timer=TSC
.NET Core SDK=2.1.403
  [Host] : .NET Core 2.1.5 (CoreCLR 4.6.26919.02, CoreFX 4.6.26919.02), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3163.0
  Core   : .NET Core 2.1.5 (CoreCLR 4.6.26919.02, CoreFX 4.6.26919.02), 64bit RyuJIT
  Mono   : Mono 5.14.0 (Visual Studio), 64bit 


```
|   Method |  Job | Runtime |        Mean |      Error |     StdDev |
|--------- |----- |-------- |------------:|-----------:|-----------:|
|  Shallow |  Clr |     Clr |    75.83 ns |  0.1226 ns |  0.1147 ns |
| OneLevel |  Clr |     Clr |    75.78 ns |  0.6750 ns |  0.5636 ns |
|     Deep |  Clr |     Clr |    75.65 ns |  0.2457 ns |  0.2298 ns |
|  Shallow | Core |    Core |   112.82 ns |  0.3834 ns |  0.3586 ns |
| OneLevel | Core |    Core |   112.27 ns |  0.2610 ns |  0.2442 ns |
|     Deep | Core |    Core |   113.04 ns |  0.5568 ns |  0.4936 ns |
|  Shallow | Mono |    Mono | 3,041.00 ns | 23.4859 ns | 21.9687 ns |
| OneLevel | Mono |    Mono | 3,017.94 ns | 46.9151 ns | 41.5890 ns |
|     Deep | Mono |    Mono | 2,943.65 ns | 12.0881 ns | 11.3072 ns |
