using System.IO;
using Xunit;

namespace Immersive.Tests
{
    public class AssemblyTests
    {
        [Fact]
        public void PeVerify()
        {
#if (DEBUG && NETFRAMEWORK)
            var beforeAssemblyPath = new DirectoryInfo(".").FullName;
            var afterAssemblyPath = new FileInfo(Fixture.AssemblyPath).Directory.FullName;

            Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
#endif
        }
    }
}
