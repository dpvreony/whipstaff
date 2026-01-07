// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Testing.Logging;

internal sealed class TestLoggerLogger : ILogger
{
    private static readonly AsyncLocal<Wrapper> _currentScopeStack = new();
    private readonly TestLogger _logger;
    private readonly string _categoryName;

    public TestLoggerLogger(string categoryName, TestLogger logger)
    {
        _logger = logger;
        _categoryName = categoryName;
    }

    private static ImmutableStack<object> CurrentScopeStack
    {
        get => _currentScopeStack.Value?.Value ?? ImmutableStack.Create<object>();
        set => _currentScopeStack.Value = new Wrapper { Value = value };
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (!_logger.IsEnabled(_categoryName, logLevel))
        {
            return;
        }

        object[] scopes = _logger.Options.IncludeScopes ? CurrentScopeStack.Reverse().ToArray() : [];
        var logEntry = new LogEntry
        {
            Date = _logger.Options.GetNow(),
            LogLevel = logLevel,
            EventId = eventId,
            State = state,
            Exception = exception,
            Formatter = (s, e) => formatter((TState)s, e),
            CategoryName = _categoryName,
            Scopes = scopes
        };

        switch (state)
        {
            case IDictionary<string, object> logDictionary:
                foreach (var property in logDictionary)
                {
                    logEntry.Properties[property.Key] = property.Value;
                }

                break;
        }

        foreach (object scope in scopes)
        {
            if (!(scope is IDictionary<string, object> scopeData))
            {
                continue;
            }

            foreach (var property in scopeData)
            {
                logEntry.Properties[property.Key] = property.Value;
            }
        }

        _logger.AddLogEntry(logEntry);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(_categoryName, logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return Push(state);
    }

#pragma warning disable GR0033 // Do not use Object in a parameter declaration.
    private static IDisposable Push(object state)
#pragma warning restore GR0033 // Do not use Object in a parameter declaration.
    {
        CurrentScopeStack = CurrentScopeStack.Push(state);
        return Disposable.Create(static () => Pop());
    }

    private static void Pop()
    {
        CurrentScopeStack = CurrentScopeStack.Pop();
    }

    private sealed class Wrapper
    {
        public required ImmutableStack<object> Value { get; init; }
    }
}
