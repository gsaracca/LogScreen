using System;
using System.Collections.Generic;

namespace _.Models;

public class OperationNode
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "";
    public List<OperationNode> Children { get; set; } = [];
    public bool IsCategory => Children.Count > 0;
}
