using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Post;

public class PostModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public string? Image { get; set; }

    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}
