using System;
using System.Linq;

public class Test0001
{
    public class Undefined
    {
        Undefined() {}
        public static readonly Undefined Instance = new();
    }

    public static TResult Let<TSource, TResult>(
        TSource source,
        Func<TSource, TResult> func)
            => func(source);

    public static char[] Select(char[] source, Func<char, char> f)
        => source.Select<char, char>(f).ToArray();

    public class IO<T>
    {
        readonly Func<T> process;
        public void ExecuteProcessFromMain() => process();

        public IO(Func<T> p) => process = p;
        public static IO<T> Return(T result) => new(() => result);

        public IO<B> OnEnd<B>(IO<B> b) => new(() =>
        {
            process();
            return b.process();
        });

        public IO<TResult> SelectMany<TResult>(Func<T, IO<TResult>> selector)
            => new(() => selector(process()).process());
    }
    public static IO<char[]> GetLine()
        => new(() => Console.ReadLine().ToCharArray());
    public static IO<Undefined> PutStrLn(char[] s) => new(() => {
        Console.WriteLine(s);
        return Undefined.Instance;
    });

    public static IO<Undefined> HaskellMain() =>
        GetLine().SelectMany((char[] s) =>
        Let(Select(s, char.ToUpper), (char[] t) =>
        s.Length == 0 ? IO<Undefined>.Return(Undefined.Instance)
                      : PutStrLn(t).OnEnd(HaskellMain())));

    public static void CSharpMain()
    {
        IO<Undefined> process = HaskellMain();
        Console.WriteLine("process created");
        process.ExecuteProcessFromMain();
    }
}
Test0001.CSharpMain();
