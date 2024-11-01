using System.Text;
using System.Diagnostics;

namespace Infinit_Int_Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> result = new List<string>();
                ExdInt try1 = new ExdInt("500000004567898685400");
                Console.WriteLine(ExdInt.root(try1,2));
            Console.WriteLine(ExdInt.Sqrt(try1));

        }
    }
    
}
//21156250000 = root(try1,2)
//22243857171 = Sqrt(try1)

public class ExdInt : IComparable<ExdInt>, IEquatable<ExdInt>
{
    #region Constructors
    public ExdInt(long intOfInput)
    {
        stringOfInt = intOfInput.ToString();
        isStringOfIntNegative = IsNegative(stringOfInt);
        ArrayOfuInt = CreateAUExdInt(stringOfInt);
    }
    public ExdInt(string stringOfInput)
    {
        stringOfInt = stringOfInput;
        isStringOfIntNegative = IsNegative(stringOfInt);
        ArrayOfuInt = CreateAUExdInt(stringOfInt);
    }
    protected ExdInt(IList<uIntWithPadding> uIntWithPadding, bool IsIntNegative = false)
    {
        ArrayOfuInt = uIntWithPadding.ToArray();
        stringOfInt = uIntWithPadding.ToString();
        isStringOfIntNegative = IsIntNegative;

    }
    protected ExdInt(ExdInt exdInt)
    {
        ArrayOfuInt = exdInt.ArrayOfuInt;
        stringOfInt = exdInt.ToString();
        isStringOfIntNegative = exdInt.isStringOfIntNegative;
    }

    protected ExdInt(ExdInt exdInt, bool IsIntNegative) 
    { 
        ArrayOfuInt = exdInt.ArrayOfuInt;
        stringOfInt = exdInt.ToString(); 
        isStringOfIntNegative = IsIntNegative;
    }

    public static implicit operator ExdInt(long intOfInput) { return new ExdInt(intOfInput); }
    public static implicit operator ExdInt(int intOfInput) { return new ExdInt(intOfInput); }
    public static implicit operator ExdInt(string stringOfInput) { return new ExdInt(stringOfInput); }

    #endregion

    #region Properties
    public bool isStringOfIntNegative;
    public uIntWithPadding[] ArrayOfuInt;
    public bool isThisNumberUsable = true;
    private string stringOfInt;
    #endregion

    #region Methods
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (isStringOfIntNegative) { sb.Append("-"); }
        bool leadingZeros = true;

        for (int i = 0; i < ArrayOfuInt.Length; i++)
        {
            string chunkString = ArrayOfuInt[i].ToString();
            if (leadingZeros)
            {
                string trimmedChunk = chunkString.TrimStart('0');
                if (trimmedChunk.Length > 0)
                {
                    sb.Append(trimmedChunk);
                    leadingZeros = false;
                }
            }
            else
            {
                sb.Append(chunkString);
            }
        }
        if (sb.Length == 0)
        {
            sb.Append("0");
        }
        return sb.ToString();
    }



    private bool AreDigitsOnly(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        foreach (char character in text)
        {
            if (character < '0' || character > '9')
                return false;
        }
        return true;
    }
    private bool IsNegative(string text)
    {
        if (text[0] == '-') 
        {
            stringOfInt= stringOfInt.Replace("-", "");
            return true;
        }

        return false;
    }

    private uIntWithPadding[] CreateAUExdInt(string stringOfInt)
    {
        if (!AreDigitsOnly(stringOfInt))
        {
            isThisNumberUsable = false;
            return null;
        }
        char[] charOfInt = stringOfInt.ToCharArray();
        int length = charOfInt.Length;
        int numChunks = (length % 9 == 0) ? (length / 9) : (length / 9) + 1;
        uIntWithPadding[] uExdInt = new uIntWithPadding[numChunks];
        int start = 0;
        int chunkLength = length % 9 == 0 ? 9 : length % 9;
        for (int i = 0; i < numChunks; i++)
        {
            uExdInt[i] = ConvertToUInt(charOfInt, start, chunkLength);
            start += chunkLength;
            chunkLength = 9; 
        }
        return uExdInt;
    }

    private static uIntWithPadding ConvertToUInt(char[] charArray, int startIndex, int length)
    {
        uint result;
        uIntWithPadding ConvertToUIntResult;

        while (length > 0)
        {
            try
            {
                if (startIndex + length > charArray.Length)
                    length = charArray.Length - startIndex;
                string str = new string(charArray, startIndex, length);
                if (uint.TryParse(str, out result))
                {
                    ConvertToUIntResult = new uIntWithPadding(result, (uint)str.Length);
                    return ConvertToUIntResult;
                }
                else
                {
                    length--;
                }
            }
            catch
            {
                return new uIntWithPadding(0, 0);
            }
        }

        return new uIntWithPadding(0, 0);
    }

    public int CompareTo(ExdInt? other)
    {
        if(other == null) { return 1; }
        if (isStringOfIntNegative && other.isStringOfIntNegative)
        {
            if (ArrayOfuInt.Length > other.ArrayOfuInt.Length) { return -1; }
            else if (ArrayOfuInt.Length < other.ArrayOfuInt.Length) { return 1; }
            else
            {
                for (int i=0; i < ArrayOfuInt.Length; i++) 
                {
                    if (ArrayOfuInt[i].Value > other.ArrayOfuInt[i].Value) { return -1; }
                    else if (ArrayOfuInt[i].Value < other.ArrayOfuInt[i].Value) { return 1; }
                }
                 return 0;
            }
        }
        else if (!isStringOfIntNegative && !other.isStringOfIntNegative)
        {
            if (ArrayOfuInt.Length > other.ArrayOfuInt.Length) { return 1; }
            else if (ArrayOfuInt.Length < other.ArrayOfuInt.Length) { return -1; }
            else
            {

                for (int i=0; i< ArrayOfuInt.Length; i++)
                {
                    if (ArrayOfuInt[i].Value > other.ArrayOfuInt[i].Value) { return 1; }
                    else if (ArrayOfuInt[i].Value < other.ArrayOfuInt[i].Value) { return -1; }
                }
                return 0;
            }
        }
        else if (isStringOfIntNegative && !other.isStringOfIntNegative)//
        {
            if (ArrayOfuInt[0].Value == other.ArrayOfuInt[0].Value)
            {
                return 0;
            }
            return -1;
        }
        else 
        {
            if (ArrayOfuInt[0].Value == other.ArrayOfuInt[0].Value)
            {
                return 0;
            }
            return 1; 
        } 
    }

    public bool Equals(ExdInt? other)
    {
        return CompareTo(other) == 0;
    }

    private static ExdInt rootHighapproachValue(ExdInt A, short n)
    {
        ulong totalDigits = 0;
        for (int i = 0; i < A.ArrayOfuInt.Length; i++)
        {
            totalDigits += A.ArrayOfuInt[i].GetTotalDigits();
        }
        ulong estimatedRootDigits = totalDigits / (ulong)n;
        if (totalDigits % (ulong)n != 0)
        {
            estimatedRootDigits += 1; 
        }

        ExdInt upperBound = new ExdInt((long)Math.Pow(10, estimatedRootDigits+1));
        return upperBound;
    }


    #endregion

    #region Operators
    public static ExdInt operator +(ExdInt a, ExdInt b)
    {
        if (a.isStringOfIntNegative != b.isStringOfIntNegative)
        {
            if (a > b)
            {
                b.isStringOfIntNegative = false;
                return new ExdInt(a - b, false);
            }
            else
            {
                a.isStringOfIntNegative = false;
                return new ExdInt(b - a, true);
            }
        }
        List<uIntWithPadding> result = new List<uIntWithPadding>();
        long carry = 0;
        int max = Math.Max(a.ArrayOfuInt.Length, b.ArrayOfuInt.Length);
        for (int i = 0; i < max; i++)
        {
            long sum = carry;
            if (i < a.ArrayOfuInt.Length)
                sum += a.ArrayOfuInt[a.ArrayOfuInt.Length - 1 - i].Value; 
            if (i < b.ArrayOfuInt.Length)
                sum += b.ArrayOfuInt[b.ArrayOfuInt.Length - 1 - i].Value;
            carry = sum / 1000000000;
            uint newValue = (uint)(sum % 1000000000);
            uint totalDigits = (i == max - 1) ? (uint)newValue.ToString().Length : 9;
            result.Add(new uIntWithPadding(newValue, totalDigits));
        }
        if (carry > 0)
        {
            result.Add(new uIntWithPadding((uint)carry, (uint)carry.ToString().Length));
        }
        result.Reverse();
        return new ExdInt(result, a.isStringOfIntNegative);
    }
    public static ExdInt operator -(ExdInt a, ExdInt b)
    {
        if (a.isStringOfIntNegative != b.isStringOfIntNegative)
        {
            if (a > b) 
            {
                b.isStringOfIntNegative = false;
                return new ExdInt(a + b);
            }
            else
            {
                b.isStringOfIntNegative = false;
                return new ExdInt(b + a);
            }
        }
        bool isResultNegative = false;
        if (a < b)
        {
            isResultNegative = true;
            var temp = a;
            a = b;
            b = temp;
        }
        List<uIntWithPadding> result = new List<uIntWithPadding>();
        long borrow = 0;
        int max = Math.Max(a.ArrayOfuInt.Length, b.ArrayOfuInt.Length);

        for (int i = 0; i < max; i++)
        {
            long diff = borrow;
            if (i < a.ArrayOfuInt.Length)
            {
                diff += a.ArrayOfuInt[a.ArrayOfuInt.Length - 1 - i].Value;
            }
            if (i < b.ArrayOfuInt.Length)
            {
                diff -= b.ArrayOfuInt[b.ArrayOfuInt.Length - 1 - i].Value;
            }
            if (diff < 0)
            {
                int borrowIndex = a.ArrayOfuInt.Length - 1 - i - 1;
                while (borrowIndex >= 0 && a.ArrayOfuInt[borrowIndex].Value == 0)
                {
                    a.ArrayOfuInt[borrowIndex].Value = 999999999;
                    borrowIndex--;
                }
                if (borrowIndex >= 0)
                {
                    a.ArrayOfuInt[borrowIndex].Value--;
                }
                diff += 1000000000;
            }

            uint newValue = (uint)(diff % 1000000000);
            uint totalDigits = (i == max - 1) ? (uint)newValue.ToString().Length : 9;
            result.Add(new uIntWithPadding(newValue, totalDigits));
        }
        result.Reverse();
        while (result.Count > 1 && result[0].Value == 0)
        {
            result.RemoveAt(0);
        }

        return new ExdInt(result, isResultNegative);
    }
    public static ExdInt operator ++(ExdInt a)
    {
        return a + 1;
    }
    public static ExdInt operator --(ExdInt a)
    {
        return a - 1;
    }
    public static ExdInt operator *(ExdInt a, ExdInt b)
    {
        bool isResultNegative = a.isStringOfIntNegative != b.isStringOfIntNegative;
        int resultLength = a.ArrayOfuInt.Length + b.ArrayOfuInt.Length;
        ulong[] resultArray = new ulong[resultLength];
        for (int i = 0; i < a.ArrayOfuInt.Length; i++)
        {
            for (int j = 0; j < b.ArrayOfuInt.Length; j++)
            {
                ulong product = (ulong)a.ArrayOfuInt[a.ArrayOfuInt.Length - 1 - i].Value * (ulong)b.ArrayOfuInt[b.ArrayOfuInt.Length - 1 - j].Value;
                resultArray[i + j] += product;
                ulong carry = resultArray[i + j] / 1000000000;
                resultArray[i + j] %= 1000000000;
                int k = i + j + 1;
                while (carry > 0 && k < resultArray.Length)
                {
                    resultArray[k] += carry;
                    carry = resultArray[k] / 1000000000;
                    resultArray[k] %= 1000000000;
                    k++;
                }
            }
        }
        int lastNonZeroIndex = resultArray.Length - 1;
        while (lastNonZeroIndex > 0 && resultArray[lastNonZeroIndex] == 0)
        {
            lastNonZeroIndex--;
        }
        if (lastNonZeroIndex < 0)
        {
            return new ExdInt("0");
        }
        List<uIntWithPadding> finalResult = new List<uIntWithPadding>();
        for (int i = lastNonZeroIndex; i >= 0; i--)
        {
            uint value = (uint)resultArray[i];
            uint totalDigits = (i == lastNonZeroIndex) ? (uint)value.ToString().Length : 9;
            finalResult.Add(new uIntWithPadding(value, totalDigits));
        }
        return new ExdInt(finalResult, isResultNegative);
    }
    public static ExdInt operator /(ExdInt a, ExdInt b)
    {
        if (b.ToString() == "0") { throw new DivideByZeroException("Cannot divide by zero."); }

        bool isResultNegative = a.isStringOfIntNegative != b.isStringOfIntNegative;
        a.isStringOfIntNegative = false;
        b.isStringOfIntNegative = false;
        if (a < b)
        {
            return new ExdInt("0");
        }

        StringBuilder resultBuilder = new StringBuilder();
        ExdInt intermediateResult = new ExdInt("0");
        int[] dividendDigits = a.ToString().Select(c => int.Parse(c.ToString())).ToArray();

        for (int i = 0; i < dividendDigits.Length; i++)
        {
            intermediateResult = (intermediateResult * 10) + dividendDigits[i];
            int quotientDigit = 0;

            while (intermediateResult >= b)
            {
                intermediateResult -= b;
                quotientDigit++;
            }

            resultBuilder.Append(quotientDigit);
        }

        string result = resultBuilder.ToString().TrimStart('0');
        return new ExdInt(result == "" ? "0" : result) { isStringOfIntNegative = isResultNegative };
    }
    public static ExdInt operator %(ExdInt a, ExdInt b)
    {
        if (b.ToString() == "0") { throw new DivideByZeroException("Cannot divide by zero."); }

        bool isResultNegative = a.isStringOfIntNegative != b.isStringOfIntNegative;
        a.isStringOfIntNegative = false;
        b.isStringOfIntNegative = false;
        if (a < b)
        {
            return new ExdInt("0");
        }

        StringBuilder resultBuilder = new StringBuilder();
        ExdInt intermediateResult = new ExdInt("0");
        int[] dividendDigits = a.ToString().Select(c => int.Parse(c.ToString())).ToArray();

        for (int i = 0; i < dividendDigits.Length; i++)
        {
            intermediateResult = (intermediateResult * 10) + dividendDigits[i];

            while (intermediateResult >= b)
            {
                intermediateResult -= b;
            }

        }

        string result = intermediateResult.ToString().TrimStart('0');
        return new ExdInt(intermediateResult) { isStringOfIntNegative = isResultNegative }; ;
    }
    public static ExdInt operator ^(ExdInt a, int b) 
    {
        if (b == 0) { return new ExdInt("1"); }
        ExdInt result = a;
        for (int i = 1; i < b; i++)
        {
            result *= a;
        }
        return result;
    }
    public static ExdInt Sqrt(ExdInt a)
    {
        if (a.isStringOfIntNegative)
        {
            throw new ArithmeticException("Cannot calculate the square root of a negative number.");
        }

        if (a == new ExdInt("0")) { return new ExdInt("0"); }
        if (a == new ExdInt("1")) { return new ExdInt("1"); }

        ExdInt low = new ExdInt("0");
        ExdInt high = a;
        ExdInt mid = null;

         while (high - low > 1)
            {
            mid = (low + high) / 2;
            ExdInt square = mid ^ 2;

            if (square == a)
            {
                return mid;
            }
            else if (square < a)
            {
                low = mid;
            }
            else
            {
                high = mid;
            }
        }
        return low;
    }
    public static ExdInt root(ExdInt a, short nthRoot)
    {
        if (a.isStringOfIntNegative)
        {
            throw new ArithmeticException("Cannot calculate the square root of a negative number.");
        }
        if (nthRoot==0)
        {
            throw new ArithmeticException("Cannot calculate the square root 0.");
        }


        if (a == new ExdInt("0")) { return new ExdInt("0"); }
        if (a == new ExdInt("1")) { return new ExdInt("1"); }

        ExdInt low = new ExdInt("0");
        ExdInt high = ExdInt.rootHighapproachValue(a ,nthRoot);
        ExdInt mid = null;

        while (high - low > 1)
        {
            mid = (low + high) / 2;
            ExdInt square = mid ^ nthRoot;
            if (square == a)
            {
                return mid;
            }
            else if (square < a)
            {
                low = mid;
            }
            else
            {
                high = mid;
            }
        }
        return low;
    }

    public static bool operator >(ExdInt operand1, ExdInt operand2)
    {
        return operand1.CompareTo(operand2) > 0;
    }
    public static bool operator <(ExdInt operand1, ExdInt operand2)
    {
        return operand1.CompareTo(operand2) < 0;
    }
    public static bool operator >=(ExdInt operand1, ExdInt operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }
    public static bool operator <=(ExdInt operand1, ExdInt operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }
    public static bool operator !=(ExdInt operand1, ExdInt operand2)
    {
        return !operand1.Equals(operand2);
    }
    public static bool operator ==(ExdInt operand1, ExdInt operand2)
    {
        if (ReferenceEquals(operand1, null) || ReferenceEquals(operand2, null))
        {
            return ReferenceEquals(operand1, operand2);
        }
        return operand1.Equals(operand2);
    }
    #endregion
}


#region uIntWithPadding
public struct uIntWithPadding
{
    public uint Value;
    private uint TotalDigits;

    public uIntWithPadding(uint value, uint totalDigits)
    {
        Value = value;
        TotalDigits = totalDigits;
    }
    public override string ToString()
    {
        return Value.ToString(TotalDigits == 9 ? "D9" : $"D{TotalDigits}");
    }
    public uint GetTotalDigits()
    {
        return TotalDigits;
    }
}
#endregion

