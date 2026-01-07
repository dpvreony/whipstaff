using System;
using ReactiveUI;

namespace Whipstaff.ReactiveUI.ReactiveCommands
{
    /// <summary>
    /// Result object for a factory that creates two reactive commands.
    /// </summary>
    /// <typeparam name="TParam1">The input type for the first command.</typeparam>
    /// <typeparam name="TResult1">The result type for the first command.</typeparam>
    /// <typeparam name="TParam2">The input type for the second command.</typeparam>
    /// <typeparam name="TResult2">The result type for the second command.</typeparam>
    /// <param name="Command1">The first command.</param>
    /// <param name="Command2">The second command.</param>
    public record ReactiveCommandFactoryResult<
        TParam1,
        TResult1,
        TParam2,
        TResult2>(ReactiveCommand<TParam1, TResult1> Command1, ReactiveCommand<TParam2, TResult2> Command2);

    /// <summary>
    /// Result object for a factory that creates two reactive commands.
    /// </summary>
    /// <typeparam name="TParam1">The input type for the first command.</typeparam>
    /// <typeparam name="TResult1">The result type for the first command.</typeparam>
    /// <typeparam name="TParam2">The input type for the second command.</typeparam>
    /// <typeparam name="TResult2">The result type for the second command.</typeparam>
    /// <typeparam name="TParam3">The input type for the third command.</typeparam>
    /// <typeparam name="TResult3">The result type for the third command.</typeparam>
    /// <param name="Command1">The first command.</param>
    /// <param name="Command2">The second command.</param>
    public record ReactiveCommandFactoryResult<
        TParam1,
        TResult1,
        TParam2,
        TResult2,
        TParam3,
        TResult3>(
        ReactiveCommand<TParam1, TResult1> Command1,
        ReactiveCommand<TParam2, TResult2> Command2,
        ReactiveCommand<TParam3, TResult3> Command3);
}
