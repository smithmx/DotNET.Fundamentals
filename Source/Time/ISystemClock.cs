﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.Time
{
    /// <summary>
    /// Defines a clock that keeps track of the current system date and time.
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// Retrieves the current system date and time
        /// </summary>
        /// <returns>The current system <see cref="DateTimeOffset">date and time</see></returns>
        DateTimeOffset GetCurrentTime();
    }
}
