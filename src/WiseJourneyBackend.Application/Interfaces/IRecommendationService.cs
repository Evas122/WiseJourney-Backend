using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

namespace WiseJourneyBackend.Application.Interfaces;
public interface IRecommendationService
{
    Task SendUserPreferencesMessageAsync(SendPreferenceMessageCommand command);
}
