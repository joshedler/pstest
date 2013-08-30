using Microsoft.VisualStudio.TestTools.UnitTesting;
using PsTest;
using Rhino.Mocks;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace PsTestTests
{
    [TestClass]
    public class PowerShellColorFormatterTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CtorNoRunspaceThrowsArgumentNullException()
        {
            // Arrange.

            // Act.

            // Assert.
            new PowerShellColorFormatter(null);
        }

        [TestMethod]
        public void CtorSavesRunspaceReference()
        {
            // Arrange.
            var expectedRunspace = MockRepository.GenerateStub<IRunspace>();

            // Act.
            var formatter = new PowerShellColorFormatter(expectedRunspace);

            // Assert.
            Assert.AreEqual(expectedRunspace, formatter.Runspace);
        }

        [TestMethod]
        public void SetColor()
        {
            // Arrange.
            const string bgColor = "bgColor";
            var bgCommand = string.Format(
                "$Host.UI.RawUI.BackgroundColor = '{0}'",
                bgColor
            );
            const string fgColor = "fgColor";
            var fgCommand = string.Format(
                "$Host.UI.RawUI.ForegroundColor = '{0}'",
                fgColor
            );

            var pipeline = MockRepository.GenerateMock<IPipeline>();
            pipeline.Expect(p => p.Invoke()).Return(null);

            var runspace = MockRepository.GenerateMock<IRunspace>();
            runspace
                .Expect(r => r.CreateNestedPipeline(fgCommand, false))
                .Return(pipeline);
            runspace
                .Expect(r => r.CreateNestedPipeline(bgCommand, false))
                .Return(pipeline);

            var formatter = new PowerShellColorFormatter(runspace);

            // Act.
            formatter.SetColor(fgColor, bgColor);

            // Assert.
            runspace.VerifyAllExpectations();
            pipeline.VerifyAllExpectations();
        }

        [TestMethod]
        public void ResetColor()
        {
            // Arrange.
            const string expectedFgColor = "fg color";
            const string expectedBgColor = "bg color";
            var psobjects = new Collection<PSObject>
            { 
                new PSObject(expectedBgColor)
            };
            string fgCommand = string.Format(
                "$Host.UI.RawUI.ForegroundColor = '{0}'",
                expectedFgColor);
            string bgCommand = string.Format(
                "$Host.UI.RawUI.BackgroundColor = '{0}'",
                expectedBgColor);

            var pipeline = MockRepository.GenerateMock<IPipeline>();
            pipeline.Expect(p => p.Invoke()).Return(psobjects);

            var runspace = MockRepository.GenerateMock<IRunspace>();
            runspace
                .Expect(r => r.CreateNestedPipeline(fgCommand, false))
                .Return(pipeline);
            runspace
                .Expect(r => r.CreateNestedPipeline(bgCommand, false))
                .Return(pipeline);

            var formatter = MockRepository.GeneratePartialMock<PowerShellColorFormatter>(runspace);
            formatter.Expect(f => f.SavedForegroundColor)
                .Return(expectedFgColor);
            formatter.Expect(f => f.SavedBackgroundColor)
                .Return(expectedBgColor);
  
            // Act.
            formatter.ResetColor();

            // Assert.
            runspace.VerifyAllExpectations();
            pipeline.VerifyAllExpectations();
        }
    }
}
