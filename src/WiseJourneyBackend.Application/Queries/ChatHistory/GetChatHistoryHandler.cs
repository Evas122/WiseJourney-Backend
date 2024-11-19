using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiseJourneyBackend.Application.Queries.ChatHistory;

public record GetChatHistoryQuery(Guid UserId);
public class GetChatHistoryHandler
{
}
