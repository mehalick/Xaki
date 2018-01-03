namespace Xaki.Tests
{
    public class TestClass : ILocalizable
    {
        [Localized]
        public string Name { get; set; }

        public TestClass(string name)
        {
            Name = name;
        }
    }
}
