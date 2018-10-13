``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.286 (1803/April2018Update/Redstone4)
AMD Ryzen 7 1800X Eight-Core Processor (Max: 3.60GHz), 1 CPU, 16 logical and 8 physical cores
Frequency=3509027 Hz, Resolution=284.9793 ns, Timer=TSC
.NET Core SDK=2.1.402
  [Host] : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3163.0
  Core   : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  Mono   : Mono 5.14.0 (Visual Studio), 64bit 


```
|   Method |  Job | Runtime |        Mean |      Error |     StdDev |
|--------- |----- |-------- |------------:|-----------:|-----------:|
|  Shallow |  Clr |     Clr |    75.88 ns |  0.2761 ns |  0.2582 ns |
| OneLevel |  Clr |     Clr |    75.93 ns |  0.2014 ns |  0.1884 ns |
|     Deep |  Clr |     Clr |    75.82 ns |  0.3290 ns |  0.3077 ns |
|  Shallow | Core |    Core |   119.37 ns |  0.2918 ns |  0.2587 ns |
| OneLevel | Core |    Core |   119.39 ns |  0.3237 ns |  0.3028 ns |
|     Deep | Core |    Core |   119.84 ns |  0.6888 ns |  0.6443 ns |
|  Shallow | Mono |    Mono | 2,943.44 ns | 17.1636 ns | 16.0549 ns |
| OneLevel | Mono |    Mono | 2,888.70 ns | 15.0383 ns | 14.0668 ns |
|     Deep | Mono |    Mono | 2,936.73 ns | 12.3258 ns | 11.5295 ns |
