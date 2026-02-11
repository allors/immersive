using System;
using System.Reflection;

using Xunit;

namespace Allors.Immersive.Tests
{
    public class ClassTests
    {
        [Fact]
        public void Default()
        {
            var type = Fixture.Assembly.GetType("Allors.Immersive.AssemblyToProcess.TestForm");
            var instance = (dynamic)Activator.CreateInstance(type);

            var constructorCalledFieldInfo = type.GetField("constructorCalled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
            var baseConstructorCalledFieldInfo = type.GetField("baseConstructorCalled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
            var assemblyConstructorCalledFieldInfo = type.GetField("assemblyConstructorCalled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);

            var button1FieldInfo = type.GetField("button1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            var textBox1FieldInfo = type.GetField("textBox1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            var nadaFieldInfo = type.GetField("nada", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            var sealedSingleFieldInfo = type.GetField("sealedSingle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            var sealedHierarchyFieldInfo = type.GetField("sealedHierarchy", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            Assert.Equal("Allors.Immersive.AssemblyToImmerse.Form", instance.GetType().BaseType.FullName);

            var constructorCalled = (bool)constructorCalledFieldInfo.GetValue(instance);
            Assert.True(constructorCalled);

            var baseConstructorCalled = (bool)baseConstructorCalledFieldInfo.GetValue(instance);
            Assert.True(baseConstructorCalled);

            var assemblyConstructorCalled = (bool)assemblyConstructorCalledFieldInfo.GetValue(instance);
            Assert.True(assemblyConstructorCalled);

            Assert.Equal("Allors.Immersive.AssemblyReferenced.Button", button1FieldInfo.FieldType.FullName);
            object button1 = button1FieldInfo.GetValue(instance);
            Assert.Equal("Allors.Immersive.AssemblyToImmerse.Button", button1.GetType().FullName);

            Assert.Equal("Allors.Immersive.AssemblyReferenced.TextBox", textBox1FieldInfo.FieldType.FullName);
            object textBox1 = textBox1FieldInfo.GetValue(instance);
            Assert.Equal("Allors.Immersive.AssemblyReferenced.TextBox", textBox1.GetType().FullName);

            Assert.Equal("Allors.Immersive.AssemblyReferenced.Nada", nadaFieldInfo.FieldType.FullName);
            object nada = nadaFieldInfo.GetValue(instance);
            Assert.Equal("Allors.Immersive.AssemblyReferenced.Nada", nada.GetType().FullName);

            Assert.Equal("Allors.Immersive.AssemblyToImmerse.SealedSingle", sealedSingleFieldInfo.FieldType.FullName);
            object sealedSingle = sealedSingleFieldInfo.GetValue(instance);
            Assert.Equal("Allors.Immersive.AssemblyToImmerse.SealedSingle", sealedSingle.GetType().FullName);

            Assert.Equal("Allors.Immersive.AssemblyToImmerse.SealedHierarchy", sealedHierarchyFieldInfo.FieldType.FullName);
            object sealedHierarchy = sealedHierarchyFieldInfo.GetValue(instance);
            Assert.Equal("Allors.Immersive.AssemblyToImmerse.SealedHierarchy", sealedHierarchy.GetType().FullName);
        }
    }
}
