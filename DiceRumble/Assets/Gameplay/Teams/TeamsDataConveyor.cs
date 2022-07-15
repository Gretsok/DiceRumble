using MOtter.DataSceneConveyance;
using System.Collections.Generic;

namespace DR.Gameplay.Teams
{
    public class TeamsDataConveyor : SceneConveyanceDataContainer
    {
        private List<Dices.DiceData> m_firstTeamDicesData = null;
        private List<Dices.DiceData> m_secondTeamDicesData = null;
        public List<Dices.DiceData> FirstTeamDicesData => m_firstTeamDicesData;
        public List<Dices.DiceData> SecondTeamDicesData => m_secondTeamDicesData;
    }
}