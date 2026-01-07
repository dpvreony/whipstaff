// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Whipstaff.Mediator.Foundatio.DistributedLocking
{
    /// <summary>
    /// Result model for subscribe and start operation.
    /// </summary>
    /// <param name="Instance">Instance of the lock process manager.</param>
    /// <param name="Task">Task for the lock process manager.</param>
    /// <param name="HasLockSubscription">Disposable for whether it has the lock subscription.</param>
    public sealed record SubscribeAndStartResultModel(DistributedLockProcessManager Instance, Task Task, IDisposable HasLockSubscription);
}
