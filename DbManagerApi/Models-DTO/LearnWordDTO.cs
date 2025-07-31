using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DbManagerApi.Models_DTO;

public class LearnWordDTO
{
    public int Id { get; set; }

    public int WordId { get; set; }

    public int LearningProgress { get; set; }

    public int UserId { get; set; }
    public LearnWordDTO(LearnWord word)
    {
        this.Id = word.Id;
        this.LearningProgress = word.LearningProgress;
        this.UserId = word.UserId;
    }
}
