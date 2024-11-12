using MediatR;

namespace WiseJourneyBackend.Application.Interfaces.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>;