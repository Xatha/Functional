namespace Functional.SumTypes;

public static class OptionTaskWrapper
{
    public static OptionTaskWrapper<T> Some<T>(T value)
    {
        return new OptionTaskWrapper<T>(value);
    }

    public static OptionTaskWrapper<T> Some<T>(Task<T> value)
    {
        return new OptionTaskWrapper<T>(value.ContinueWith(async t => Option.Some(await t)).Unwrap());
    }

    public static OptionTaskWrapper<T> None<T>()
    {
        return new OptionTaskWrapper<T>();
    }
}

public readonly struct OptionTaskWrapper<T>
{
    private readonly Task<Option<T>>? _task;

    internal OptionTaskWrapper(Task<Option<T>> task)
    {
        _task = task;
    }

    internal OptionTaskWrapper(T value) : this(Task.FromResult(Option.Some(value)))
    {
    }

    public OptionTaskWrapper<TResult> Map<TResult>(Func<Task<T>, Task<TResult>> func)
    {
        if (_task is null) return new OptionTaskWrapper<TResult>();

        var nextTask = _task.ContinueWith(async t => await t switch
        {
            (true, var value) => SumTypes.Option.Some(await func(Task.FromResult(value))),
            _ => SumTypes.Option.None<TResult>()
        }).Unwrap();

        return new OptionTaskWrapper<TResult>(nextTask);
    }

    public OptionTaskWrapper<TResult> Map<TResult>(Func<Task<T>, Task<Option<TResult>>> func)
    {
        if (_task is null) return new OptionTaskWrapper<TResult>();

        var nextTask = _task.ContinueWith(async t => await t switch
        {
            (true, var value) => await func(Task.FromResult(value)),
            _ => Option.None<TResult>()
        }).Unwrap();

        return new OptionTaskWrapper<TResult>(nextTask);
    }

    public Task<Option<T>> Resolve()
    {
        return _task ?? Task.FromResult(Option.None<T>());
    }

    public void Deconstruct(out bool isSome, out T value)
    {
        if (_task is not null)
        {
            var option = _task.Result;
            (isSome, value) = option;
        }
        else
        {
            isSome = false;
            value = default!;
        }
    }

    public OptionTaskWrapper<TResult> Cast<TResult>()
    {
        return _task is not null
            ? new OptionTaskWrapper<TResult>(_task.ContinueWith(async t =>
            {
                var option = await t;

                return option switch
                {
                    (true, var value) => value is TResult res ? Option.Some(res) : Option.None<TResult>(),
                    _ => Option.None<TResult>()
                };
            }).Unwrap())
            : new OptionTaskWrapper<TResult>();
    }
}