// --------------------------------------------------------------------------------------------------
// <copyright file="IQuery.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions.CQRS;

/// <summary>
/// Query interface with return for CQRS pattern.
/// </summary>
/// <typeparam name="TRes">Return Type.</typeparam>
public interface IQuery<TRes> : ICommand<TRes>
{
}
