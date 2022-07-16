using MOtter.DataSceneConveyance;
using System.Collections.Generic;

namespace DR.Gameplay.Teams
{
    public class TeamsDataConveyor : SceneConveyanceDataContainer
    {
        public List<Dices.DiceData> FirstTeamDicesData { get; set; }
        public List<Dices.DiceData> SecondTeamDicesData { get; set; }
    }
}