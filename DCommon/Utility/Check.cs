using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DCommon.Utility
{
    public sealed class Check
    {
        #region Interface

        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PreconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PreconditionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PreconditionException("Precondition failed.");
            }
            else
            {
                Trace.Assert(assertion, "Precondition failed.");
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PostconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PostconditionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PostconditionException("Postcondition failed.");
            }
            else
            {
                Trace.Assert(assertion, "Postcondition failed.");
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new InvariantException(message);
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new InvariantException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new InvariantException("Invariant failed.");
            }
            else
            {
                Trace.Assert(assertion, "Invariant failed.");
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new AssertionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new AssertionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new AssertionException("Assertion failed.");
            }
            else
            {
                Trace.Assert(assertion, "Assertion failed.");
            }
        }

        /// <summary>
        /// Set this if you wish to use Trace Assert statements 
        /// instead of exception handling. 
        /// (The Check class uses exception handling by default.)
        /// </summary>
        public static bool UseAssertions
        {
            get
            {
                return useAssertions;
            }
            set
            {
                useAssertions = value;
            }
        }

        /// <summary>
        /// Makes sure the parameter is not null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        [DebuggerStepThrough]
        public static void IsNotNull(object value, string name) { if (value == null) throw new ArgumentNullException(name); }

        /// <summary>
        /// Checks if <paramref name="argument"/> is of type <paramref name="baseType"/>.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="baseType"></param>
        /// <param name="argumentName"></param>
        [DebuggerStepThrough]
        public static void IsOfType(object argument, Type baseType, string argumentName)
        {
            IsNotNull(argument, argumentName);
            IsNotNull(baseType, "baseType");

            if (!baseType.IsAssignableFrom(argument.GetType()))
                throw new ArgumentException("{0} is not of type {1}.".FormatWith(argument, baseType), argumentName);
        }

        /// <summary>
        /// Checks if the count of the <see cref="IEnumerable{T}"/> is in the specified range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="argumentName"></param>
        public static void ElementCountInRange<T>(IEnumerable<T> enumerable, int min, int max, string argumentName)
        {
            IsNotNull(enumerable, argumentName);
            int count = enumerable.Count();
            if (count < min || count > max)
                throw new ArgumentOutOfRangeException("the number of items <{0}> is not in the expected range min <{1}> max <{2}>".FormatWith(count, min, max), argumentName);
        }

        /// <summary>
        /// Checks ifthe string is null or empty.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(argumentName, "string is null or empty");
        }

        public static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
        {
            if (param == null)
            {
                return !checkForNull;
            }
            param = param.Trim();
            return (((!checkIfEmpty || (param.Length >= 1)) && ((maxSize <= 0) || (param.Length <= maxSize))) && (!checkForCommas || !param.Contains(",")));
        }

        public static void ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
            else
            {
                param = param.Trim();
                if (checkIfEmpty && (param.Length < 1))
                {
                    throw new ArgumentException(string.Format("The parameter '{0}' must not be empty.", paramName));
                }
                if ((maxSize > 0) && (param.Length > maxSize))
                {
                    throw new ArgumentException(string.Format("The parameter '{0}' is too long: it must not exceed {1} chars in length.",
                        paramName, maxSize.ToString(CultureInfo.InvariantCulture)));
                }
                if (checkForCommas && param.Contains(","))
                {
                    throw new ArgumentException(string.Format("The parameter '{0}' must not contain commas.", paramName));
                }
            }
        }

        public static void ValidateArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if (param.Length < 1)
            {
                throw new ArgumentException(string.Format("The array parameter '{0}' should not be empty.", paramName));
            }
            Hashtable hashtable = new Hashtable(param.Length);
            for (int i = param.Length - 1; i >= 0; i--)
            {
                ValidateParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
                if (hashtable.Contains(param[i]))
                {
                    throw new ArgumentException(string.Format("The array '{0}' should not contain duplicate values.", paramName));
                }
                hashtable.Add(param[i], param[i]);
            }
        }

        #endregion // Interface

        #region Implementation

        // No creation
        private Check() { }

        /// <summary>
        /// Is exception handling being used?
        /// </summary>
        private static bool UseExceptions
        {
            get
            {
                return !useAssertions;
            }
        }

        // Are trace assertion statements being used? 
        // Default is to use exception handling.
        private static bool useAssertions;

        #endregion // Implementation
    }

    #region Exceptions

    /// <summary>
    /// Exception raised when a contract is broken.
    /// Catch this exception type if you wish to differentiate between 
    /// any DesignByContract exception and other runtime exceptions.
    ///  
    /// </summary>
    [Serializable]
    public class DesignByContractException : ApplicationException
    {
        protected DesignByContractException() { }
        protected DesignByContractException(string message) : base(message) { }
        protected DesignByContractException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when a precondition fails.
    /// </summary>
    [Serializable]
    public class PreconditionException : DesignByContractException
    {
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException() { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message) : base(message) { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when a postcondition fails.
    /// </summary>
    [Serializable]
    public class PostconditionException : DesignByContractException
    {
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException() { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message) : base(message) { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when an invariant fails.
    /// </summary>
    [Serializable]
    public class InvariantException : DesignByContractException
    {
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException() { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message) : base(message) { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when an assertion fails.
    /// </summary>
    [Serializable]
    public class AssertionException : DesignByContractException
    {
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException() { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message) : base(message) { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message, Exception inner) : base(message, inner) { }
    }

    #endregion
}
