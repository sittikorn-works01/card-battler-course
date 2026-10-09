using UnityEngine;

namespace CardBattlerCourse.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float maxHealth;
        public int attackPower;
        public int empowerAmount;
        public int blockAmount;
    }
}