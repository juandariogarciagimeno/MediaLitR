// --------------------------------------------------------------------------------------------------
// <copyright file="ICommand.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions.CQRS;

using MediaLitr.Abstractions.Models;

/// <summary>
/// Command interface with return for CQRS pattern.
/// </summary>
/// <typeparam name="TRes">Return value.</typeparam>
public interface ICommand<out TRes>
{
}

/// <summary>
/// Command interface without return for CQRS pattern.
/// </summary>
public interface ICommand : ICommand<Unit>
{
}
