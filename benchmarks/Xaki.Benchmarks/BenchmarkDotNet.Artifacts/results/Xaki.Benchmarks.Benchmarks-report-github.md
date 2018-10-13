``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4)
AMD Ryzen 7 1800X Eight-Core Processor (Max: 3.60GHz), 1 CPU, 16 logical and 8 physical cores
Frequency=3509031 Hz, Resolution=284.9790 ns, Timer=TSC
.NET Core SDK=2.1.402
  [Host] : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3132.0
  Core   : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  Mono   : Mono 5.14.0 (Visual Studio), 64bit 


```
|   Method |  Job | Runtime |        Mean |      Error |     StdDev |
|--------- |----- |-------- |------------:|-----------:|-----------:|
|  Shallow |  Clr |     Clr |    71.68 ns |  0.2210 ns |  0.2067 ns |
| OneLevel |  Clr |     Clr |    72.53 ns |  0.2572 ns |  0.2406 ns |
|     Deep |  Clr |     Clr |    72.17 ns |  0.1634 ns |  0.1528 ns |
|  Shallow | Core |    Core |   120.25 ns |  0.5492 ns |  0.4586 ns |
| OneLevel | Core |    Core |   119.29 ns |  0.7832 ns |  0.6540 ns |
|     Deep | Core |    Core |   119.68 ns |  0.5247 ns |  0.4651 ns |
|  Shallow | Mono |    Mono | 2,929.89 ns | 15.8602 ns | 14.8357 ns |
| OneLevel | Mono |    Mono | 2,938.11 ns | 10.9115 ns |  9.1116 ns |
|     Deep | Mono |    Mono | 2,940.46 ns |  9.4142 ns |  8.3455 ns |
