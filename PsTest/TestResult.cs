using System;

namespace PsTest
{
    /// <summary>
    /// Represents the result of an invoked unit test.
    /// </summary>
    public class TestResult
    {
        private readonly string _testName;
        private readonly bool _success;
        private readonly Exception _unexpectedException;

        /// <summary>
        /// Initializes a new instance of the TestResult class with the
        /// specified test name and success status.
        /// </summary>
        /// <param name="testName">
        /// The name of the test.
        /// </param>
        /// <param name="success">
        /// True if the test was a success or false.
        /// </param>
        /// <param name="unexpectedException">
        /// Any unexpected Exception that occurred during test execution.
        /// </param>
        public TestResult(string testName, bool success, Exception unexpectedException = null)
        {
            if (testName == null)
            {
                throw new ArgumentNullException("testName");
            }
            _testName = testName;
            _success = success;
            _unexpectedException = unexpectedException;
        }

        /// <summary>
        /// Get the name of the test.
        /// </summary>
        public string TestName { get { return _testName; } }

        /// <summary>
        /// Get a value indicating if the test was successful or not.
        /// </summary>
        public bool Success { get { return _success; } }

        /// <summary>
        /// Get any unexpected Exception that occurred during test execution.
        /// </summary>
        public Exception Exception { get { return _unexpectedException; } }
    }
}
