using System.Collections.Generic;

namespace MarsRobots.Entities
{
    public interface IRobot
    {
        Position CurrentPosition { get; set; }
        int Id { get; set; }
        Position InitialPosition { get; }
        List<string> InstructionStrings { get; set; }
        bool isLost { get; set; }
        string Name { get; set; }
    }
}